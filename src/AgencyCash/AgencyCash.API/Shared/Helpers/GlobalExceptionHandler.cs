using System.Diagnostics.CodeAnalysis;
using AgencyCash.API.Shared.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace AgencyCash.API.Shared.Helpers;

[ExcludeFromCodeCoverage(Justification = "Global exception handler is tested indirectly through integration tests.")]
public sealed class GlobalExceptionHandler(Fanex.Logging.ILogger logger) : IExceptionHandler
{
    private readonly Fanex.Logging.ILogger logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Request.EnableBuffering();
        await logger.ErrorAsync(await ExceptionHelper.BuildError(httpContext.Request, exception));

        var response = httpContext.Response;
        response.ContentType = "application/json";
        response.StatusCode = StatusCodes.Status500InternalServerError;
        await response.WriteAsJsonAsync(new Result(StatusCodes.Status500InternalServerError, "Internal Server Error"), cancellationToken);

        return true;
    }
}
