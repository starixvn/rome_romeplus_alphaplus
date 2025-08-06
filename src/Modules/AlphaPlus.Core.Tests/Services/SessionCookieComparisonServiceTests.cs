using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Services;
using AlphaPlus.Core.Models;
using Fanex.Logging;

namespace AlphaPlus.Core.Tests.Services;

public class SessionCookieComparisonServiceTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IUserSession> _userSessionMock;
    private readonly SessionCookieComparisonOptions _options;
    private readonly SessionCookieComparisonService _service;

    public SessionCookieComparisonServiceTests()
    {
        _loggerMock = new Mock<ILogger>();
        _userSessionMock = new Mock<IUserSession>();
        _options = new SessionCookieComparisonOptions();
        
        _userSessionMock.Setup(x => x.CustId).Returns(123);
        _userSessionMock.Setup(x => x.UserName).Returns("testuser");
        _userSessionMock.Setup(x => x.NetSessionId).Returns("456");
        _userSessionMock.Setup(x => x.RoleId).Returns(1);
        _userSessionMock.Setup(x => x.AdminId).Returns(0);
        _userSessionMock.Setup(x => x.AdminName).Returns("");
        _userSessionMock.Setup(x => x.UserLoginIp).Returns("192.168.1.1");

        var optionsMock = new Mock<IOptions<SessionCookieComparisonOptions>>();
        optionsMock.Setup(x => x.Value).Returns(_options);

        _service = new SessionCookieComparisonService(_loggerMock.Object, _userSessionMock.Object, optionsMock.Object);
    }

    [Fact]
    public void IsMonitoredPage_ShouldReturnTrue_ForMonitoredPaths()
    {
        // Arrange
        var paths = new[]
        {
            "/settings/creditbalance",
            "/reports/winlossdetail"
        };

        // Act & Assert
        foreach (var path in paths)
        {
            var result = _service.IsMonitoredPage(new PathString(path));
            Assert.True(result, $"Path {path} should be monitored");
        }
    }

    [Fact]
    public void IsMonitoredPage_ShouldReturnFalse_ForOtherPaths()
    {
        // Arrange
        var paths = new[]
        {
            "/home",
            "/login",
            "/dashboard",
            "/settings",
            "/reports/other",
            "/credit",
            "/balance"
        };

        // Act & Assert
        foreach (var path in paths)
        {
            var result = _service.IsMonitoredPage(new PathString(path));
            Assert.False(result, $"Path {path} should not be monitored");
        }
    }

    [Fact]
    public void ExtractSessionCookieFromRequest_ShouldReturnCookie_WhenExists()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Cookie"] = ".AspNetCore.Session=test-session-id";

        // Act
        var result = _service.ExtractSessionCookieFromRequest(context.Request);

        // Assert
        Assert.Equal("test-session-id", result);
    }

    [Fact]
    public void ExtractSessionCookieFromRequest_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Headers["Cookie"] = "";

        // Act
        var result = _service.ExtractSessionCookieFromRequest(context.Request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ExtractSessionCookieFromResponse_ShouldReturnCookie_WhenExists()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Headers["Set-Cookie"] = ".AspNetCore.Session=test-session-id; path=/; httponly";

        // Act
        var result = _service.ExtractSessionCookieFromResponse(context.Response);

        // Assert
        Assert.Equal("test-session-id", result);
    }

    [Fact]
    public void ExtractSessionCookieFromResponse_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Headers["Set-Cookie"] = "other-cookie=value; path=/; httponly";

        // Act
        var result = _service.ExtractSessionCookieFromResponse(context.Response);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldLog_WhenCookiesAreDifferent()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.Headers["Referer"] = "https://example.com";
        context.Request.Headers["User-Agent"] = "Mozilla/5.0";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldNotLog_WhenCookiesAreSame()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "same-cookie", "same-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldNotLog_ForNonMonitoredPages()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/home";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldNotLog_WhenFeatureDisabled()
    {
        // Arrange
        _options.EnableSessionCookieComparison = false;
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData("Error", "ErrorAsync")]
    [InlineData("Info", "InfoAsync")]
    [InlineData("Debug", "DebugAsync")]
    [InlineData("Warning", "WarnAsync")]
    [InlineData("Unknown", "WarnAsync")]
    public async Task CompareAndLogSessionCookiesAsync_ShouldUseCorrectLogLevel(string logLevel, string expectedMethod)
    {
        // Arrange
        _options.LogLevel = logLevel;
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        switch (expectedMethod)
        {
            case "ErrorAsync":
                _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Once);
                break;
            case "InfoAsync":
                _loggerMock.Verify(x => x.InfoAsync(It.IsAny<string>()), Times.Once);
                break;
            case "DebugAsync":
                _loggerMock.Verify(x => x.DebugAsync(It.IsAny<string>()), Times.Once);
                break;
            case "WarnAsync":
                _loggerMock.Verify(x => x.WarnAsync(It.IsAny<string>()), Times.Once);
                break;
        }
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldIncludeUserInfo_InLogMessage()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.Is<string>(msg => 
            msg.Contains("User: testuser (ID: 123)") && 
            msg.Contains("Session: 456") && 
            msg.Contains("Role: 1"))), Times.Once);
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldIncludePathInfo_InLogMessage()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.QueryString = new QueryString("?param=value");

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.Is<string>(msg => 
            msg.Contains("Path: /reports/winlossdetail?param=value"))), Times.Once);
    }

    [Fact]
    public async Task CompareAndLogSessionCookiesAsync_ShouldIncludeCookieInfo_InLogMessage()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        // Act
        await _service.CompareAndLogSessionCookiesAsync(context, "request-cookie", "response-cookie");

        // Assert
        _loggerMock.Verify(x => x.ErrorAsync(It.Is<string>(msg => 
            msg.Contains("Request Cookie: request-cookie") && 
            msg.Contains("Response Cookie: response-cookie"))), Times.Once);
    }
} 