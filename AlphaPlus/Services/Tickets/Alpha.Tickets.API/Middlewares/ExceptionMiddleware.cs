using System.Data.SqlClient;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Web;
using Fanex.Data;

namespace Alpha.Tickets.API.Middlewares;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate next;
    private readonly Fanex.Logging.ILogger logger;

    public ExceptionMiddleware(RequestDelegate next, Fanex.Logging.ILogger logger)
        => (this.next, this.logger) = (next, logger);

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            httpContext.Request.EnableBuffering();
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        response.ContentType = "application/json";

        await logger.ErrorAsync(await BuildError(context.Request, exception), exception);
        await response.WriteAsync(JsonSerializer.Serialize(new { response.StatusCode, exception.Message }));
    }

    private static async Task<string> BuildError(HttpRequest request, Exception exception)
    {
        var errorBuilder = new StringBuilder();
        errorBuilder
            .AppendLine(exception.Message).AppendLine()
            .Append("PATH: ").AppendLine($"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}")
            .AppendLine()
            .Append("BODY: ").AppendLine(await GetRawBodyString(request))
            .AppendLine()
            .AppendLine("HEADERS:")
            .AppendLine(GetHeaderValues(request))
            .AppendLine()
            .AppendLine("COOKIES:")
            .AppendLine(GetCookiesValues(request))
            .AppendLine();

        // if (exception is ObjectDbException objectDbException
        //     && exception.InnerException is SqlException sqlException)
        // {
        //     BuildDbException(errorBuilder, objectDbException, sqlException);
        // }

        errorBuilder
            .Append("EXCEPTION: ")
            .Append(exception.ToString());

        return errorBuilder.ToString();
    }

    private static Task<string> GetRawBodyString(HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, Encoding.UTF8);
        return reader.ReadToEndAsync();
    }

    private static string GetHeaderValues(HttpRequest request)
        => string.Join(Environment.NewLine, request.Headers.Select(header => $"{header.Key}:{header.Value}"));

    private static string GetCookiesValues(HttpRequest request)
        => string.Join(Environment.NewLine, request.Cookies.Select(header => $"{header.Key}:{header.Value}"));

    // private static void BuildDbException(
    //     StringBuilder errorBuilder,
    //     ObjectDbException dbException,
    //     SqlException sqlException)
    // {
    //     errorBuilder
    //         .AppendLine("DATABASE INFO:")
    //         .Append("Server: ").AppendLine(sqlException.Server)
    //         .Append("DbName: ").AppendLine(dbException.DbName)
    //         .Append("SpName: ").AppendLine(dbException.SpName)
    //         .AppendLine("Parameters: ");

    //     foreach (var item in dbException.Parameters)
    //     {
    //         errorBuilder
    //             .Append(item.Key)
    //             .Append('=')
    //             .AppendLine(HttpUtility.HtmlEncode(item.Value.ToString()));
    //     }

    //     errorBuilder
    //         .Append("LineNumber: ").AppendLine(sqlException.LineNumber.ToString())
    //         .AppendLine();
    // }
}
