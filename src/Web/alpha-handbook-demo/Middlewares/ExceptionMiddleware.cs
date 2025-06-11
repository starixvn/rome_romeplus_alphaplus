using System.Net;
using Newtonsoft.Json;

namespace Alpha.Handbook.Demo.Middlewares;

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
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (context.Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Code = 500, Message = "Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [500]!" }));
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Redirect("~/error");
        }
    }
}