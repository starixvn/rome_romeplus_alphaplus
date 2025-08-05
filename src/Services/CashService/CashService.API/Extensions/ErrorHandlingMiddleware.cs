using System.Text.Json;
using AlphaPlus.CashService.API.Features.Shared.Helpers;
using Microsoft.AspNetCore.Diagnostics;

namespace AlphaPlus.CashService.API.Extensions;

public static class ErrorHandlingMiddleware
{
    public static async Task HandleError(HttpContext context, WebApplication app)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        if (exception != null)
        {
            var logger = app.Services.GetService<Fanex.Logging.ILogger>();
            await logger.ErrorAsync(await ExceptionHelper.BuildError(context.Request, exception), exception);
        }

        var result = JsonSerializer.Serialize(new
        {
            error = exception?.Message ?? "An error occurred",
            statusCode = StatusCodes.Status500InternalServerError
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync(result);
    }
}