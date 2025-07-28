using System.Net;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.PartialUM.Exceptions;
using Fanex.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace AlphaPlus.Core.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger logger, IUserSession userSession)
{
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            httpContext.Request.EnableBuffering();
            await next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        if (exception is PartialUMException partialUMException)
        {
            context.Response.Redirect(MicroSites.MainUrl($"~/partialum?umfrom={partialUMException.StartTime}&umto={partialUMException.EndTime}"));
            return;
        }

        context.Items["customData"] = new
        {
            userSession.CustId,
            userSession.UserName,
            userSession.RoleId,
            userSession.SubAccId,
            userSession.SubUserName,
            userSession.AdminId,
            userSession.AdminName
        };

        if (!exception.Message.Contains("Vendor Api Error, Unspecified Error", StringComparison.OrdinalIgnoreCase))
        {
            await logger.ErrorAsync(await ExceptionHelper.BuildError(context.Request, exception));
        }

        if (context.Request.Headers.XRequestedWith == "XMLHttpRequest")
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Code = 500,
                Message = "Sorry, there was an unexpected issue on the server. Please seek help from our customer support. [500]!"
            }));
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Redirect(MicroSites.AuthUrl("~/error"));
        }
    }
}