using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using AlphaPlus.PartialUM.Exceptions;

namespace AlphaPlus.Tickets.API.Middlewares;

[ExcludeFromCodeCoverage]
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Fanex.Logging.ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, Fanex.Logging.ILogger logger)
        => (_next, _logger) = (next, logger);

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            httpContext.Request.EnableBuffering();
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleException(httpContext, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        object apiException;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (exception is PartialUMException partialUMException)
        {
            apiException = new { Code = PartialUMException.ErrorCode, partialUMException.StartTime, partialUMException.EndTime };
        }
        else
        {
            apiException = new { Code = context.Response.StatusCode, Message = $"An error occurred. {exception.Message})" };
            await _logger.ErrorAsync(await ExceptionHelper.BuildError(context.Request, exception));
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(apiException));
    }
}