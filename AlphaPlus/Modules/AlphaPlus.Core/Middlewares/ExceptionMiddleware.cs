using System.Net;
using AlphaPlus.Core.Helpers;
using AlphaPlus.PartialUM.Exceptions;
using Fanex.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AlphaPlus.Core.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
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
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var ex = exceptionHandlerFeature.Error;
        await _logger.ErrorAsync(await ExceptionHelper.BuildError(context.Request, exception, exceptionHandlerFeature.Path));

        if (ex is PartialUMException partialUMException)
        {
            context.Response.Redirect(MicroSites.MainUrl($"~/partialum?umfrom={partialUMException.StartTime}&umto={partialUMException.EndTime}"));
        }
        else
        {
            if (context.Request.Headers.XRequestedWith == "XMLHttpRequest")
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Code = 500, Message = "Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [500]!" }));
            }
            else
            {
                context.Response.ContentType = "text/html";
                context.Response.Redirect(MicroSites.MainUrl("~/error"));
            }
        }
    }
}