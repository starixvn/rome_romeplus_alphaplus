using Microsoft.AspNetCore.Http;
using AlphaPlus.Core.Services;

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
            var originalFromHeader = context.Request.Headers["X-Original-Session"].ToString();
            var fromCookie = context.Request.Cookies["X-Response-Session"];
            
            if (!string.IsNullOrWhiteSpace(originalFromHeader) && !string.IsNullOrWhiteSpace(fromCookie))
            {
                await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, originalFromHeader, fromCookie, "session_cookie_mismatch: from LB/HAProxy");
            }

            await _next(context);
        }
        finally
        {
            var responseSessionCookie = _sessionCookieComparisonService.ExtractSessionCookieFromResponse(context.Response);
            await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, requestSessionCookie, responseSessionCookie, "session_cookie_mismatch: from Server");
        }
    }
}