using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
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
        if (request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            agentId = GetAgentId(request.Query);
        }
        else
        {
            requestBody = await GetRequestBodyString(request);
            if (!string.IsNullOrWhiteSpace(requestBody))
            {
                agentId = GetAgentId(requestBody);
                if (EndpointHelper.IsRegisterCashMember(functionName))
                {
                    requestBody = RemovePassword(requestBody);
                }
            }
        }

        var criteria = new AddRequestLoggingCriteria(
            agentId,
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

            Console.WriteLine(JsonHelper.ToJson(criteria));

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
        var hasPassword = false;
        var node = JsonNode.Parse(requestBody)?.AsObject();

        if (node != null)
        {
            foreach (var prop in node)
            {
                if (prop.Key.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    hasPassword = true;
                    node[prop.Key] = "******";
                }
            }
        }

        return hasPassword
            ? node.ToJsonString(new JsonSerializerOptions { WriteIndented = false })
            : requestBody;
    }

    private static int GetAgentId(IQueryCollection query)
        => query?.Count > 0
        && query.TryGetValue("agentId", out var agentIdValue)
        && int.TryParse(agentIdValue, out int agentId)
            ? agentId
            : 0;

    private static int GetAgentId(string requestBody)
    {
        try
        {
            using var doc = JsonDocument.Parse(requestBody.ToLowerInvariant());
            return doc.RootElement.TryGetProperty("agentid", out var element)
                && element.TryGetInt32(out int agentId)
                ? agentId
                : 0;
        }
        catch (JsonException)
        {
            // Invalid JSON or property not found
        }

        return 0;
    }
}
