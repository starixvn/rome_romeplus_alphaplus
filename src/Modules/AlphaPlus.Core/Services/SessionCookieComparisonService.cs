using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Fanex.Logging;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Core.Services;

public class SessionCookieComparisonService : ISessionCookieComparisonService
{
    private readonly ILogger _logger;
    private readonly IUserSession _userSession;
    private readonly SessionCookieComparisonOptions _options;

    public SessionCookieComparisonService(
        ILogger logger, 
        IUserSession userSession,
        IOptions<SessionCookieComparisonOptions> options)
    {
        _logger = logger;
        _userSession = userSession;
        _options = options.Value;
    }

    public async Task CompareAndLogSessionCookiesAsync(HttpContext context, string requestCookie, string responseCookie)
    {
        if (!_options.EnableSessionCookieComparison)
            return;

        if (!IsMonitoredPage(context.Request.Path))
            return;

        if (!string.Equals(requestCookie, responseCookie, StringComparison.OrdinalIgnoreCase))
        {
            var logMessage = BuildLogMessage(context, requestCookie, responseCookie);
            
            switch (_options.LogLevel.ToLowerInvariant())
            {
                case "error":
                    await _logger.ErrorAsync(logMessage);
                    break;
                case "info":
                    await _logger.InfoAsync(logMessage);
                    break;
                case "debug":
                    await _logger.DebugAsync(logMessage);
                    break;
                default:
                    await _logger.WarnAsync(logMessage);
                    break;
            }
        }
    }

    public bool IsMonitoredPage(PathString path)
    {
        var pathString = path.Value?.ToLowerInvariant() ?? "";
        
        var allPatterns = _options.MonitoredPaths;

        return allPatterns.Any(pattern => pathString.Contains(pattern.ToLowerInvariant()));
    }

    public string ExtractSessionCookieFromRequest(HttpRequest request)
    {
        return request.Cookies[".AspNetCore.Session"];
    }

    public string ExtractSessionCookieFromResponse(HttpResponse response)
    {
        var setCookieHeaders = response.Headers["Set-Cookie"];
        
        foreach (var setCookieHeader in setCookieHeaders)
        {
            if (string.IsNullOrEmpty(setCookieHeader))
                continue;

            var sessionCookie = ParseSessionCookieFromSetCookieHeader(setCookieHeader);
            if (!string.IsNullOrEmpty(sessionCookie))
                return sessionCookie;
        }

        return null;
    }

    private static string ParseSessionCookieFromSetCookieHeader(string setCookieHeader)
    {
        if (setCookieHeader.Contains(".AspNetCore.Session="))
        {
            var startIndex = setCookieHeader.IndexOf(".AspNetCore.Session=");
            var valueStart = startIndex + ".AspNetCore.Session=".Length;
            var endIndex = setCookieHeader.IndexOf(';', valueStart);
            
            if (endIndex == -1)
                endIndex = setCookieHeader.Length;
            
            return setCookieHeader.Substring(valueStart, endIndex - valueStart);
        }

        return null;
    }

    private string BuildLogMessage(HttpContext context, string requestCookie, string responseCookie)
    {
        var userInfo = $"User: {_userSession.UserName} (ID: {_userSession.CustId})";
        var sessionInfo = $"Session: {_userSession.NetSessionId}";
        var roleInfo = $"Role: {_userSession.RoleId}";
        var adminInfo = _userSession.AdminId > 0 ? $"Admin: {_userSession.AdminName} (ID: {_userSession.AdminId})" : "";
        var pathInfo = $"Path: {context.Request.Path}{context.Request.QueryString}";
        var cookieInfo = $"Request Cookie: {requestCookie ?? "null"} | Response Cookie: {responseCookie ?? "null"}";

        var logParts = new List<string>
        {
            $"Server Side Comparison: Session cookie mismatch detected. {pathInfo}. {userInfo}. {sessionInfo}. {roleInfo}."
        };

        if (!string.IsNullOrEmpty(adminInfo))
            logParts.Add(adminInfo);

        if (_options.IncludeIpAddressInLog)
            logParts.Add($"IP: {_userSession.UserLoginIp}");

        if (_options.IncludeRefererInLog)
            logParts.Add($"Referer: {context.Request.Headers["Referer"]}");

        if (_options.IncludeUserAgentInLog)
            logParts.Add($"UserAgent: {context.Request.Headers["User-Agent"]}");

        logParts.Add(cookieInfo);

        return string.Join(" ", logParts);
    }
} 