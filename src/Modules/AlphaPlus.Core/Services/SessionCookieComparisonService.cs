using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Fanex.Logging;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Configurations;
using System.Text.Json;

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

    public async Task CompareAndLogSessionCookiesAsync(HttpContext context, string requestCookie, string responseCookie, string messageTitle)
    {
        if (!_options.EnableSessionCookieComparison)
            return;

        if (!IsMonitoredPage(context.Request.Path))
            return;

        if (!string.Equals(requestCookie, responseCookie, StringComparison.OrdinalIgnoreCase))
        {
            var logMessage = BuildLogMessage(context, requestCookie, responseCookie, messageTitle);
            
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
        return request.Cookies["X-Response-Session"];
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
        if (setCookieHeader.Contains("X-Response-Session="))
        {
            var startIndex = setCookieHeader.IndexOf("X-Response-Session=");
            var valueStart = startIndex + "X-Response-Session=".Length;
            var endIndex = setCookieHeader.IndexOf(';', valueStart);
            
            if (endIndex == -1)
                endIndex = setCookieHeader.Length;
            
            return setCookieHeader.Substring(valueStart, endIndex - valueStart);
        }

        return null;
    }

    private string BuildLogMessage(HttpContext context, string requestCookie, string responseCookie, string messageTitle)
    {
        var logData = new
        {
            Event = messageTitle,
            OriginalSessionId = requestCookie,
            NewSessionId = responseCookie,
            Timestamp = DateTime.UtcNow,
            Url = context.Request.Path,
            Referer = context.Request.Headers["Referer"],
            UserAgent = context.Request.Headers["User-Agent"],
            CustId = _userSession.CustId,
            Username = _userSession.UserName,
            SubAccId = _userSession.SubAccId,
            AdminId = _userSession.AdminId,
            AdminName = _userSession.AdminName,
            RoleId = _userSession.RoleId,
            IpAddress = _userSession.UserLoginIp
        };

        return $"Server Side Comparison: Session Cookie Mismatch:{Environment.NewLine}{JsonSerializer.Serialize(logData, new JsonSerializerOptions { WriteIndented = true })}";
    }
}