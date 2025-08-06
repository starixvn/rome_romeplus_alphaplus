using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Core.Services;

public interface ISessionCookieComparisonService
{
    Task CompareAndLogSessionCookiesAsync(HttpContext context, string requestCookie, string responseCookie);
    bool IsMonitoredPage(PathString path);
    string ExtractSessionCookieFromRequest(HttpRequest request);
    string ExtractSessionCookieFromResponse(HttpResponse response);
} 