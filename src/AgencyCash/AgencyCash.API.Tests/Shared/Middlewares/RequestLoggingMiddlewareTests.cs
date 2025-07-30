using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Criteria;
using AgencyCash.API.Shared.Middlewares;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AgencyCash.API.Tests.Shared.Middlewares;

public class RequestLoggingMiddlewareTests
{
    private readonly Mock<Fanex.Logging.ILogger> _loggerMock;
    private readonly Mock<IRequestLoggingService> _requestLoggingServiceMock;
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly RequestLoggingMiddleware _middleware;

    public RequestLoggingMiddlewareTests()
    {
        _loggerMock = new Mock<Fanex.Logging.ILogger>();
        _requestLoggingServiceMock = new Mock<IRequestLoggingService>();
        _nextMock = new Mock<RequestDelegate>();
        _middleware = new RequestLoggingMiddleware(
            _nextMock.Object,
            _loggerMock.Object,
            _requestLoggingServiceMock.Object);
    }

    [Fact]
    public async Task Invoke_WhenPathDoesNotStartWithApi_ShouldSkipLogging()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/not-api/endpoint";

        // Act
        await _middleware.Invoke(context);

        // Assert
        _nextMock.Verify(next => next(context), Times.Once);
        _requestLoggingServiceMock.Verify(x => x.WriteLog(It.IsAny<AddRequestLoggingCriteria>()), Times.Never);
    }

    [Fact]
    public async Task Invoke_WhenPathStartsWithApi_ShouldLogRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/endpoint";
        context.Request.Method = "GET";
        context.Request.Scheme = "https";
        context.Request.Host = new HostString("test.com");
        context.Response.Body = new MemoryStream();

        _nextMock.Setup(next => next(It.IsAny<HttpContext>()))
            .Returns(Task.CompletedTask);

        // Act
        await _middleware.Invoke(context);

        // Assert
        _requestLoggingServiceMock.Verify(x => x.WriteLog(It.IsAny<AddRequestLoggingCriteria>()), Times.Once);
    }

    [Fact]
    public async Task Invoke_WhenExceptionOccurs_ShouldLogErrorAndReturn500()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/endpoint";
        context.Request.Method = "GET";
        context.Response.Body = new MemoryStream();

        var exception = new Exception("Test exception");
        _nextMock.Setup(next => next(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);

        AddRequestLoggingCriteria capturedCriteria = null;
        _requestLoggingServiceMock.Setup(x => x.WriteLog(It.IsAny<AddRequestLoggingCriteria>()))
            .Callback<AddRequestLoggingCriteria>(criteria => capturedCriteria = criteria);

        // Act
        await _middleware.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        _loggerMock.Verify(l => l.ErrorAsync(It.IsAny<string>()), Times.Once);

        Assert.NotNull(capturedCriteria);
        Assert.Equal(StatusCodes.Status500InternalServerError, capturedCriteria.StatusCode);
        Assert.Contains(exception.ToString(), capturedCriteria.Exception);
    }
}
