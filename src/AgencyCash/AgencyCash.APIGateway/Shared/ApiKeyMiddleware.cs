namespace AgencyCash.APIGateway.Shared;

public sealed class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private readonly RequestDelegate next = next;
    private readonly IConfiguration configuration = configuration;

    public async Task InvokeAsync(HttpContext context)
    {
        var userApiKey = context.Request.Headers[ApiKeyHeaderName];
        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            await WriteError(context.Response, StatusCodes.Status400BadRequest, $"{ApiKeyHeaderName} is required.");
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
