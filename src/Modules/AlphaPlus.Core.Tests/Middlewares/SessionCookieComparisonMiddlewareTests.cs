using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Services;
using AlphaPlus.Core.Middlewares;
using AlphaPlus.Core.Models;
using Fanex.Logging;

namespace AlphaPlus.Core.Tests.Middlewares;

public class SessionCookieComparisonMiddlewareTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IUserSession> _userSessionMock;
    private readonly Mock<ISessionCookieComparisonService> _serviceMock;
    private readonly SessionCookieComparisonOptions _options;

    public SessionCookieComparisonMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger>();
        _userSessionMock = new Mock<IUserSession>();
        _serviceMock = new Mock<ISessionCookieComparisonService>();
        _options = new SessionCookieComparisonOptions();
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallServiceMethods()
    {
        // Arrange
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => await Task.CompletedTask,
            _serviceMock.Object);

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";
        context.Request.Headers["Cookie"] = ".AspNetCore.Session=test-session-id";

        _serviceMock.Setup(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()))
            .Returns("different-session-id");
        _serviceMock.Setup(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _serviceMock.Verify(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()), Times.Once);
        _serviceMock.Verify(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()), Times.Once);
        _serviceMock.Verify(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleNullCookies()
    {
        // Arrange
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => await Task.CompletedTask,
            _serviceMock.Object);

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        _serviceMock.Setup(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()))
            .Returns((string)null);
        _serviceMock.Setup(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()))
            .Returns((string)null);
        _serviceMock.Setup(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _serviceMock.Verify(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), null, null), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallService_WhenResponseBodyIsWritten()
    {
        // Arrange
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                await context.Response.WriteAsync("test response body");
            },
            _serviceMock.Object);

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        _serviceMock.Setup(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _serviceMock.Verify(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleException_AndStillCallService()
    {
        // Arrange
        var middleware = new SessionCookieComparisonMiddleware(
            (context) =>
            {
                throw new Exception("Test exception");
            },
            _serviceMock.Object);

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        _serviceMock.Setup(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => middleware.InvokeAsync(context));

        // Verify service was still called
        _serviceMock.Verify(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallService_WhenEmptyResponseBody()
    {
        // Arrange
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                // Don't write anything to response
                await Task.CompletedTask;
            },
            _serviceMock.Object);

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        _serviceMock.Setup(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _serviceMock.Verify(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallService_WhenLargeResponseBody()
    {
        // Arrange
        var responseBody = new string('x', 10000); // 10KB response
        var middleware = new SessionCookieComparisonMiddleware(
            async (context) => 
            {
                await context.Response.WriteAsync(responseBody);
            },
            _serviceMock.Object);

        var context = new DefaultHttpContext();
        context.Request.Path = "/reports/winlossdetail";

        _serviceMock.Setup(x => x.ExtractSessionCookieFromRequest(It.IsAny<HttpRequest>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.ExtractSessionCookieFromResponse(It.IsAny<HttpResponse>()))
            .Returns("test-session-id");
        _serviceMock.Setup(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        _serviceMock.Verify(x => x.CompareAndLogSessionCookiesAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
} 