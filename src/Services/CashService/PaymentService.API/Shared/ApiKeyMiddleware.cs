using Microsoft.AspNetCore.Authorization;

namespace AlphaPlus.PaymentService.API.Shared;

public class ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private const string ApiKeyHeaderName = "X-API-Key";
    private const string ApiKeyName = "ApiKey";
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
        {
            await _next(context);
            return;
        }

        var userApiKey = context.Request.Headers[ApiKeyHeaderName];
        if (string.IsNullOrWhiteSpace(userApiKey))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var apiKey = _configuration.GetValue<string>(ApiKeyName);
        if (userApiKey != apiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}