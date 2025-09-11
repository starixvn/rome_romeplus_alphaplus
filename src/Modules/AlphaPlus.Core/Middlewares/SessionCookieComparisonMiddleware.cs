using Microsoft.AspNetCore.Http;
using AlphaPlus.Core.Services;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Core.Middlewares;

public class SessionCookieComparisonMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ISessionCookieComparisonService _sessionCookieComparisonService;
    private readonly IUserSession _userSession;
    public SessionCookieComparisonMiddleware(RequestDelegate next, ISessionCookieComparisonService sessionCookieComparisonService, IUserSession userSession)
    {
        _next = next;
        _sessionCookieComparisonService = sessionCookieComparisonService;
        _userSession = userSession;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestSessionCookie = _sessionCookieComparisonService.ExtractSessionCookieFromRequest(context.Request);
        var isValidRequestCookie = Guid.TryParse(requestSessionCookie, out _);
        try
        {
            var fromServer = context.Session.Id;
            
            if (!string.IsNullOrWhiteSpace(fromServer) && !string.IsNullOrWhiteSpace(requestSessionCookie) && _userSession.IsLogged && isValidRequestCookie)
            {
                await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, fromServer, requestSessionCookie, "session_cookie_mismatch: from LB/HAProxy");
            }

            await _next(context);
        }
        finally
        {
            var responseSessionCookie = _sessionCookieComparisonService.ExtractSessionCookieFromResponse(context.Response);

            if (!string.IsNullOrWhiteSpace(requestSessionCookie) && !string.IsNullOrWhiteSpace(responseSessionCookie) && _userSession.IsLogged && isValidRequestCookie)
            {
                await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, requestSessionCookie, responseSessionCookie, "session_cookie_mismatch: from Server");
            }
        }
    }
}