using System.Diagnostics.CodeAnalysis;
using System.Net;

[SuppressMessage("SonarQube", "S3903:Types should be defined in named namespaces", Justification = "This is a static class that contains extension methods for IEnumerable<T>.")]
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
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await _logger.ErrorAsync(await ExceptionHelper.BuildError(context.Request, exception));
        await context.Response.WriteAsync(new
        {
            context.Response.StatusCode,
            exception.Message
        }.ToString());
    }
}
