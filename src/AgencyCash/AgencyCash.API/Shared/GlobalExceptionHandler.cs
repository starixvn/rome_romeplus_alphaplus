using Microsoft.AspNetCore.Diagnostics;

namespace AgencyCash.API.Shared;

public sealed class GlobalExceptionHandler(Fanex.Logging.ILogger logger) : IExceptionHandler
{
    private readonly Fanex.Logging.ILogger logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await logger.ErrorAsync(await ExceptionHelper.BuildError(httpContext.Request, exception));

        var response = httpContext.Response;
        response.ContentType = "application/json";
        response.StatusCode = StatusCodes.Status500InternalServerError;
        await response.WriteAsJsonAsync(new { response.StatusCode, exception.Message }, cancellationToken: cancellationToken);

        return false;
    }
}
