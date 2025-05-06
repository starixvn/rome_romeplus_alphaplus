using Microsoft.AspNetCore.Diagnostics;
using System.Text;

namespace Agency.APIGateway.Shared;

public sealed class GlobalExceptionHandler(Fanex.Logging.ILogger logger) : IExceptionHandler
{
    private const string BreakLine = "\r\n";
    private const string TwoBreakLine = "\r\n\r\n";
    private readonly Fanex.Logging.ILogger logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await logger.ErrorAsync(await BuildError(httpContext.Request, exception));

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new
        {
            httpContext.Response.StatusCode,
            exception.Message
        }, cancellationToken: cancellationToken);

        return false;
    }

    private static async Task<string> BuildError(HttpRequest request, Exception exception)
    {
        request.EnableBuffering();

        var errorBuilder = new StringBuilder();
        errorBuilder
            .Append(exception.Message)
            .Append(TwoBreakLine)
            .Append($"PATH: {request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}")
            .Append(TwoBreakLine)
            .Append($"REFERER: {request.Headers.Referer}")
            .Append(TwoBreakLine)
            .Append($"BODY: {await GetRawBodyStringAsync(request)}")
            .Append(TwoBreakLine)
            .Append("HEADERS:").Append(BreakLine)
            .Append(GetHeaderValues(request))
            .Append(TwoBreakLine)
            .Append("COOKIES:").Append(BreakLine)
            .Append(GetCookiesValues(request))
            .Append(TwoBreakLine)
            .Append($"EXCEPTION: {exception.ToString()}");

        return errorBuilder.ToString();
    }

    private static async Task<string> GetRawBodyStringAsync(HttpRequest request)
    {
        request.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }

    private static string GetHeaderValues(HttpRequest request)
        => string.Join(BreakLine, request.Headers.Select(header => $"{header.Key}:{header.Value}"));

    private static string GetCookiesValues(HttpRequest request)
        => string.Join(BreakLine, request.Cookies.Select(header => $"{header.Key}:{header.Value}"));
}
