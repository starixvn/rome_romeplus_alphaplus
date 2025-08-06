using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AlphaPlus.Core.Models;
using Fanex.Logging;
using Main.Site.Features.Account;

namespace AlphaPlus.Main.Site.UnitTests.Features.Account;

public static class SessionControllerTestHelpers
{
    public static SessionMismatchLogModel CreateValidLogData()
    {
        return new SessionMismatchLogModel
        {
            Event = "session_mismatch",
            OriginalSessionId = "original_session_id",
            NewSessionId = "new_session_id",
            Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            Url = "https://example.com/test",
            Referrer = "https://example.com",
            RetryCount = 1,
            MaxRetries = 3
        };
    }

    public static SessionMismatchLogModel CreateLogDataWithCustomValues(
        string eventName = "session_mismatch",
        string originalSessionId = "original_session_id",
        string newSessionId = "new_session_id",
        string timestamp = null!,
        string url = "https://example.com/test",
        string referrer = "https://example.com",
        int retryCount = 1,
        int maxRetries = 3)
    {
        return new SessionMismatchLogModel
        {
            Event = eventName,
            OriginalSessionId = originalSessionId,
            NewSessionId = newSessionId,
            Timestamp = timestamp ?? DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            Url = url,
            Referrer = referrer,
            RetryCount = retryCount,
            MaxRetries = maxRetries
        };
    }

    public static HeaderDictionary CreateHeaders(string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36")
    {
        return new HeaderDictionary
        {
            ["User-Agent"] = userAgent
        };
    }

    public static void SetupUserSession(Mock<IUserSession> userSessionMock, 
        int userId = 123, 
        string userName = "testuser", 
        string siteName = "TestSite", 
        string userLoginIp = "192.168.1.1")
    {
        userSessionMock.Setup(x => x.UserId).Returns(userId);
        userSessionMock.Setup(x => x.UserName).Returns(userName);
        userSessionMock.Setup(x => x.SiteName).Returns(siteName);
        userSessionMock.Setup(x => x.UserLoginIp).Returns(userLoginIp);
    }

    public static void SetupHttpContext(Mock<HttpContext> httpContextMock, string userAgent = "Test User Agent")
    {
        var headers = CreateHeaders(userAgent);
        httpContextMock.Setup(x => x.Request.Headers).Returns(headers);
    }

    public static void SetupLogger(Mock<ILogger> loggerMock, bool shouldThrow = false, string errorMessage = "Logger error")
    {
        if (shouldThrow)
        {
            loggerMock.Setup(x => x.ErrorAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception(errorMessage));
        }
        else
        {
            loggerMock.Setup(x => x.ErrorAsync(It.IsAny<string>()))
                .Returns(Task.CompletedTask);
        }
    }

    public static SessionController CreateController(
        Mock<ILogger> loggerMock, 
        Mock<IUserSession> userSessionMock, 
        Mock<HttpContext> httpContextMock)
    {
        var controller = new SessionController(loggerMock.Object, userSessionMock.Object);
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContextMock.Object
        };

        return controller;
    }

    public static (Mock<ILogger> LoggerMock, Mock<IUserSession> UserSessionMock, Mock<HttpContext> HttpContextMock, SessionController Controller) 
        CreateTestSetup(
            int userId = 123,
            string userName = "testuser",
            string siteName = "TestSite",
            string userLoginIp = "192.168.1.1",
            string userAgent = "Test User Agent",
            bool loggerShouldThrow = false)
    {
        var loggerMock = new Mock<ILogger>();
        var userSessionMock = new Mock<IUserSession>();
        var httpContextMock = new Mock<HttpContext>();

        SetupUserSession(userSessionMock, userId, userName, siteName, userLoginIp);
        SetupHttpContext(httpContextMock, userAgent);
        SetupLogger(loggerMock, loggerShouldThrow);

        var controller = CreateController(loggerMock, userSessionMock, httpContextMock);

        return (loggerMock, userSessionMock, httpContextMock, controller);
    }
} 