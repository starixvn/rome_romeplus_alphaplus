using System.Text;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Helpers;

public static class ExceptionHelper
{
    private const string BreakLine = "\r\n";
    private const string TwoBreakLine = "\r\n\r\n";

    public static async Task<string> BuildError(HttpRequest request, Exception exception, string requestedUrl)
    {
        var errorBuilder = new StringBuilder();
        errorBuilder
            .Append(exception.Message).Append(TwoBreakLine)
            .Append("PATH: ")
            .Append($"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}")
            .Append(TwoBreakLine)
            .Append($"REQUESTED URL: {requestedUrl}").Append(TwoBreakLine)
            .Append("BODY: ")
            .Append(await GetRawBodyStringAsync(request))
            .Append(TwoBreakLine)
            .Append("HEADERS:").Append(BreakLine)
            .Append(GetHeaderValues(request))
            .Append(TwoBreakLine)
            .Append("COOKIES:").Append(BreakLine)
            .Append(GetCookiesValues(request))
            .Append(TwoBreakLine);

        errorBuilder
            .Append("EXCEPTION: ")
            .Append(exception.ToString());

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

    private static string GetSpParameters(IDictionary<string, object> parameters)
        => string.Join(BreakLine, parameters.Select(p => $"{p.Key}={p.Value}"));
}