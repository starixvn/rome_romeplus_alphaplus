using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using AlphaPlus.Core.Models;
using Fanex.Logging;
using Main.Site.Features.Account;

namespace AlphaPlus.Main.Site.UnitTests.Features.Account;

[Trait("Category", "Account")]
public class SessionControllerTests
{
    private readonly Mock<ILogger> _loggerMock;
    private readonly Mock<IUserSession> _userSessionMock;
    private readonly Mock<HttpContext> _httpContextMock;
    private readonly SessionController _controller;

    public SessionControllerTests()
    {
        var (loggerMock, userSessionMock, httpContextMock, controller) = SessionControllerTestHelpers.CreateTestSetup();
        _loggerMock = loggerMock;
        _userSessionMock = userSessionMock;
        _httpContextMock = httpContextMock;
        _controller = controller;
    }

    [Fact]
    public async Task LogSessionMismatch_WithValidData_ShouldReturnOkResult()
    {
        // Arrange
        var logData = SessionControllerTestHelpers.CreateValidLogData();
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        
        SessionControllerTestHelpers.SetupHttpContext(_httpContextMock, userAgent);
        SessionControllerTestHelpers.SetupUserSession(_userSessionMock);
        SessionControllerTestHelpers.SetupLogger(_loggerMock);

        // Act
        var result = await _controller.LogSessionMismatch(logData);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        
        response.Should().NotBeNull();
        var successProperty = response.GetType().GetProperty("success");
        var messageProperty = response.GetType().GetProperty("message");
        
        successProperty.Should().NotBeNull();
        messageProperty.Should().NotBeNull();
        
        successProperty!.GetValue(response).Should().Be(true);
        messageProperty!.GetValue(response).Should().Be("Session mismatch logged successfully");

        // Verify that the log data was populated with session information
        logData.UserId.Should().Be(123);
        logData.Username.Should().Be("testuser");
        logData.SiteLabel.Should().Be("TestSite");
        logData.IpAddress.Should().Be("192.168.1.1");
        logData.UserAgent.Should().Be(userAgent);

        // Verify logger was called
        _loggerMock.Verify(x => x.ErrorAsync(It.Is<string>(msg => 
            msg.Contains("Client Side Comparison: Session Cookie Mismatch") && 
            msg.Contains("testuser") && 
            msg.Contains("TestSite") && 
            msg.Contains("192.168.1.1") && 
            msg.Contains(userAgent))), Times.Once);
    }

    [Fact]
    public async Task LogSessionMismatch_WhenLoggerThrowsException_ShouldReturnInternalServerError()
    {
        // Arrange
        var logData = SessionControllerTestHelpers.CreateValidLogData();
        SessionControllerTestHelpers.SetupHttpContext(_httpContextMock);
        SessionControllerTestHelpers.SetupUserSession(_userSessionMock);
        // Setup logger to throw on first call, but succeed on second call (in catch block)
        _loggerMock.SetupSequence(x => x.ErrorAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Logger error"))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.LogSessionMismatch(logData);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        statusCodeResult.StatusCode.Should().Be(500);
        
        var response = statusCodeResult.Value;
        var successProperty = response.GetType().GetProperty("success");
        var messageProperty = response.GetType().GetProperty("message");
        
        successProperty.Should().NotBeNull();
        messageProperty.Should().NotBeNull();
        
        successProperty!.GetValue(response).Should().Be(false);
        messageProperty!.GetValue(response).Should().Be("Failed to log session mismatch");

        // Verify logger was called twice (once for the session mismatch, once for the error)
        _loggerMock.Verify(x => x.ErrorAsync(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public async Task LogSessionMismatch_WithEmptyUserAgent_ShouldHandleGracefully()
    {
        // Arrange
        var logData = SessionControllerTestHelpers.CreateValidLogData();
        var headers = new HeaderDictionary(); // Empty headers

        _httpContextMock.Setup(x => x.Request.Headers).Returns(headers);
        SessionControllerTestHelpers.SetupUserSession(_userSessionMock);
        SessionControllerTestHelpers.SetupLogger(_loggerMock);

        // Act
        var result = await _controller.LogSessionMismatch(logData);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        
        var successProperty = response.GetType().GetProperty("success");
        successProperty.Should().NotBeNull();
        successProperty!.GetValue(response).Should().Be(true);
        logData.UserAgent.Should().Be(string.Empty); // Should be empty string when header is not found

        // Verify logger was called
        _loggerMock.Verify(x => x.ErrorAsync(It.Is<string>(msg => 
            msg.Contains("Client Side Comparison: Session Cookie Mismatch") && 
            msg.Contains("testuser") && 
            msg.Contains("TestSite") && 
            msg.Contains("192.168.1.1"))), Times.Once);
    }

    [Fact]
    public async Task LogSessionMismatch_ShouldPopulateAllSessionDataCorrectly()
    {
        // Arrange
        var logData = SessionControllerTestHelpers.CreateValidLogData();
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        
        SessionControllerTestHelpers.SetupHttpContext(_httpContextMock, userAgent);
        SessionControllerTestHelpers.SetupUserSession(_userSessionMock, userId: 456, userName: "john_doe", siteName: "AlphaSite", userLoginIp: "10.0.0.1");
        SessionControllerTestHelpers.SetupLogger(_loggerMock);

        // Act
        var result = await _controller.LogSessionMismatch(logData);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        
        // Verify all session data was populated correctly
        logData.UserId.Should().Be(456);
        logData.Username.Should().Be("john_doe");
        logData.SiteLabel.Should().Be("AlphaSite");
        logData.IpAddress.Should().Be("10.0.0.1");
        logData.UserAgent.Should().Be(userAgent);

        // Verify the original data is preserved
        logData.Event.Should().Be("session_mismatch");
        logData.OriginalSessionId.Should().Be("original_session_id");
        logData.NewSessionId.Should().Be("new_session_id");
        logData.RetryCount.Should().Be(1);
        logData.MaxRetries.Should().Be(3);
    }
} 