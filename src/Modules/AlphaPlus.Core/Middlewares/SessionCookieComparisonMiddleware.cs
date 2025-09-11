using Microsoft.AspNetCore.Http;
using AlphaPlus.Core.Services;
using System.Web;

namespace AlphaPlus.Core.Middlewares;

public class SessionCookieComparisonMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISessionCookieComparisonService _sessionCookieComparisonService;

    public SessionCookieComparisonMiddleware(RequestDelegate next, ISessionCookieComparisonService sessionCookieComparisonService)
    {
        _next = next;
        _sessionCookieComparisonService = sessionCookieComparisonService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestSessionCookie = _sessionCookieComparisonService.ExtractSessionCookieFromRequest(context.Request);

        try
        {
            var fromServer = context.Session.Id;
            var fromRequestCookie = HttpUtility.UrlEncode(context.Request.Cookies["X-Response-Session"]);
            
            if (!string.IsNullOrWhiteSpace(fromServer) && !string.IsNullOrWhiteSpace(fromRequestCookie))
            {
                await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, fromServer, fromRequestCookie, "session_cookie_mismatch: from LB/HAProxy");
            }

            await _next(context);
        }
        finally
        {
            var responseSessionCookie = _sessionCookieComparisonService.ExtractSessionCookieFromResponse(context.Response);
            if (!string.IsNullOrWhiteSpace(responseSessionCookie))
            {
                await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, requestSessionCookie, responseSessionCookie, "session_cookie_mismatch: from Server");
            }
        }
    }
}