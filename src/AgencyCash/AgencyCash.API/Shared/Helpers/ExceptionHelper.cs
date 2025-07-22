using System.Diagnostics.CodeAnalysis;
using System.Text;
using Fanex.Data;
using Microsoft.Data.SqlClient;

namespace AgencyCash.API.Shared.Helpers;

[ExcludeFromCodeCoverage(Justification = "This class is a simple helper used for log global exception and does not require code coverage.")]
public static class ExceptionHelper
{
    private const string BreakLine = "\r\n";
    private const string TwoBreakLine = "\r\n\r\n";

    public static string BuildError(HttpRequest request, Exception exception, string path, string requestBody)
    {
        var errorBuilder = new StringBuilder();
        errorBuilder
            .Append(exception.Message).Append(TwoBreakLine)
            .Append($"PATH: ${path}")
            .Append(TwoBreakLine)
            .Append($"BODY: ${requestBody}")
            .Append(TwoBreakLine)
            .Append("HEADERS:").Append(BreakLine)
            .Append(GetHeaderValues(request))
            .Append(TwoBreakLine);

        if (exception is ObjectDbException objectDbException
            && exception.InnerException is SqlException sqlException)
        {
            errorBuilder.Append(BuildDbException(objectDbException, sqlException));
        }

        errorBuilder.Append($"EXCEPTION: ${exception.ToString()}");

        return errorBuilder.ToString();
    }

    private static string GetHeaderValues(HttpRequest request)
        => string.Join(BreakLine, request.Headers.Select(header => $"{header.Key}:{header.Value}"));

    private static string BuildDbException(ObjectDbException dbException, SqlException sqlException)
    {
        var errorBuilder = new StringBuilder();
        errorBuilder
            .Append("DATABASE INFO:").Append(BreakLine)
            .Append("Server: ").Append(sqlException.Server).Append(BreakLine)
            .Append("DbName: ").Append(dbException.DbName).Append(BreakLine)
            .Append("SpName: ").Append(dbException.SpName).Append(BreakLine)
            .Append("Parameters: ").Append(BreakLine).Append(GetSpParameters(dbException.Parameters)).Append(BreakLine)
            .Append("Line: ").Append(sqlException.LineNumber).Append(TwoBreakLine);

        return errorBuilder.ToString();
    }

    private static string GetSpParameters(IDictionary<string, object> parameters)
        => string.Join(BreakLine, parameters.Select(p => $"{p.Key}={p.Value}"));
}