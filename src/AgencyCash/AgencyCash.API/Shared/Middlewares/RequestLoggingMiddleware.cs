using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using AgencyCash.API.Shared.Criteria;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Shared.Middlewares;

public class RequestLoggingMiddleware(
    RequestDelegate next,
    Fanex.Logging.ILogger logger,
    IRequestLoggingService requestLoggingService)
{
    private readonly RequestDelegate next = next;
    private readonly Fanex.Logging.ILogger logger = logger;
    private readonly IRequestLoggingService requestLoggingService = requestLoggingService;

    public async Task Invoke(HttpContext context)
    {
        var request = context.Request;
        var path = request.Path;
        if (!path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var functionName = EndpointHelper.ToFunctionName(path);
        var requestUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{path}{request.QueryString}";
        var requestBody = string.Empty;
        var agentId = 0;
        var custId = 0;
        if (request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            (agentId, custId) = GetAgentAndCustId(request.Query);
        }
        else
        {
            requestBody = await GetRequestBodyString(request);
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                (agentId, custId) = GetAgentAndCustId(requestBody);
                if (EndpointHelper.IsRegisterCashMember(functionName))
                {
                    requestBody = RemovePassword(requestBody);
                }
            }
        }

        var criteria = new AddRequestLoggingCriteria(
            agentId,
            custId,
            functionName,
            requestUrl,
            requestBody);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await next(context);
            criteria.StatusCode = context.Response.StatusCode;
        }
        catch (Exception ex)
        {
            const string errorMessage = "Internal Server Error";
            await logger.ErrorAsync(ExceptionHelper.BuildError(request, ex, criteria.RequestUrl, criteria.RequestBody));

            criteria.StatusCode = StatusCodes.Status500InternalServerError;
            criteria.Exception = ex.ToString();
            criteria.ResponseBody = errorMessage;

            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = StatusCodes.Status500InternalServerError;
            await response.WriteAsJsonAsync(new Result(StatusCodes.Status500InternalServerError, errorMessage));
        }
        finally
        {
            stopwatch.Stop();
            criteria.Duration = (int)stopwatch.ElapsedMilliseconds;
            var bodyStream = context.Response.Body;

            if (string.IsNullOrWhiteSpace(criteria.ResponseBody))
            {
                bodyStream.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(bodyStream, Encoding.UTF8, leaveOpen: true);
                criteria.ResponseBody = await reader.ReadToEndAsync();
            }

            bodyStream.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);

            _ = Task.Run(() => requestLoggingService.WriteLog(criteria));
        }
    }

    private static async Task<string> GetRequestBodyString(HttpRequest request)
    {
        request.EnableBuffering();

        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);

        return requestBody;
    }

    private static string RemovePassword(string requestBody)
    {
        try
        {
            var node = JsonNode.Parse(requestBody)?.AsObject();
            if (node == null)
            {
                return requestBody;
            }

            if (node.TryGetPropertyValue("password", out _))
            {
                node["password"] = "******";
                return node.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
            }

            return requestBody;
        }
        catch (JsonException)
        {
            return requestBody;
        }
    }

    private static (int, int) GetAgentAndCustId(IQueryCollection query)
    {
        if (query == null || query.Count == 0)
        {
            return (0, 0);
        }

        var agentId = 0;
        var custId = 0;

        _ = query.TryGetValue("agentId", out var agentIdValue) && int.TryParse(agentIdValue, out agentId);
        _ = query.TryGetValue("custId", out var custIdValue) && int.TryParse(custIdValue, out custId);

        return (agentId, custId);
    }

    private static (int, int) GetAgentAndCustId(string requestBody)
    {
        var agentId = 0;
        var custId = 0;

        try
        {
            using var doc = JsonDocument.Parse(requestBody.ToLowerInvariant());

            if (doc.RootElement.TryGetProperty("agentid", out var agentElement))
            {
                agentElement.TryGetInt32(out agentId);
            }

            if (doc.RootElement.TryGetProperty("custid", out var custElement))
            {
                custElement.TryGetInt32(out custId);
            }
        }
        catch (JsonException)
        {
            // Invalid JSON or property not found
        }

        return (agentId, custId);
    }
}
