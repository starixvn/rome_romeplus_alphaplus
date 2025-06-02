namespace AgencyCash.APIGateway.Shared;

public sealed class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private readonly RequestDelegate next = next;
    private readonly IConfiguration configuration = configuration;

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        // Skip API key check for Swagger UI and static files
        if (path != null &&
            (path.StartsWith("/swagger") ||
             path.StartsWith("/agency-cash-api/swagger") ||
             path.StartsWith("/agency-cash-api/index.html") ||
             path.StartsWith("/agency-cash-api/favicon.ico") ||
             path.StartsWith("/agency-cash-api/index.css") ||
             path.StartsWith("/agency-cash-api/index.js") ||
             path.StartsWith("/agency-cash-api/favicon-32x32.png") ||
             path.StartsWith("/agency-cash-api/favicon-16x16.png") ||
             path.Contains("swagger-ui") ||
             path.Contains("swagger.json") ||
             path.Contains("swagger-initializer.js") ||
             path.Contains("swagger-ui.css") ||
             path.Contains("swagger-ui-bundle.js") ||
             path.Contains("swagger-ui-standalone-preset.js")
            ))
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var userApiKey) || string.IsNullOrWhiteSpace(userApiKey))
        {
            await WriteError(context.Response, StatusCodes.Status401Unauthorized, $"{ApiKeyHeaderName} is required.");
            return;
        }

        if (userApiKey != configuration["ApiKey"])
        {
            await WriteError(context.Response, StatusCodes.Status401Unauthorized, $"{ApiKeyHeaderName} is invalid.");
            return;
        }

        await next(context);
    }

    private static Task WriteError(HttpResponse response, int statusCode, string error)
    {
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        return response.WriteAsync(error);
    }
}
