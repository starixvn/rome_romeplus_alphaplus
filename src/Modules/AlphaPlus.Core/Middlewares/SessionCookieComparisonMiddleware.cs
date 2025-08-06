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
        
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);
        }
        finally
        {
            var responseSessionCookie = _sessionCookieComparisonService.ExtractSessionCookieFromResponse(context.Response);
            
            await _sessionCookieComparisonService.CompareAndLogSessionCookiesAsync(context, requestSessionCookie, responseSessionCookie);
            
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
} 