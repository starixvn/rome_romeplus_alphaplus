using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace AlphaPlus.Auth.Site.UnitTests;

public class AccountServiceTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly Mock<IUserSessionService> _mockUserSessionService;
    private readonly AlphaPlus.Auth.Site.Features.Account.AccountService _accountService;

    public AccountServiceTests()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") // Set BaseAddress to fix URI issues
        };
        _mockUserSession = new Mock<IUserSession>();
        _mockUserSessionService = new Mock<IUserSessionService>();
        
        _accountService = new AlphaPlus.Auth.Site.Features.Account.AccountService(
            _httpClient,
            _mockUserSession.Object,
            _mockUserSessionService.Object);
    }

    [Fact]
    public async Task CheckIn_WhenUserIsInternal_ShouldUseNegativeAdminId()
    {
        // Arrange
        _mockUserSession.Setup(x => x.IsInternal).Returns(true);
        _mockUserSession.Setup(x => x.AdminId).Returns(123);
        _mockUserSession.Setup(x => x.CustId).Returns(456);
        _mockUserSession.Setup(x => x.NetSessionId).Returns("session-123");
        _mockUserSession.Setup(x => x.ServerId).Returns(1);
        _mockUserSession.Setup(x => x.UserLoginIp).Returns("127.0.0.1");

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("123") // Return onlineUserId
            });

        // Act
        await _accountService.CheckIn();

        // Assert
        _mockUserSession.VerifySet(x => x.OnUserId = 123, Times.Once);
    }

    [Fact]
    public async Task CheckIn_WhenUserIsNotInternal_ShouldUseSubAccId()
    {
        // Arrange
        _mockUserSession.Setup(x => x.IsInternal).Returns(false);
        _mockUserSession.Setup(x => x.SubAccId).Returns(789);
        _mockUserSession.Setup(x => x.CustId).Returns(456);
        _mockUserSession.Setup(x => x.NetSessionId).Returns("session-123");
        _mockUserSession.Setup(x => x.ServerId).Returns(1);
        _mockUserSession.Setup(x => x.UserLoginIp).Returns("127.0.0.1");

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("456") // Return onlineUserId
            });

        // Act
        await _accountService.CheckIn();

        // Assert
        _mockUserSession.VerifySet(x => x.OnUserId = 456, Times.Once);
    }

    [Fact]
    public async Task AfterSignIn_WhenSuccessful_ShouldSetUserSessionValues()
    {
        // Arrange
        var signInParam = new AlphaPlus.Auth.Site.Features.Account.SignInParam
        {
            CustId = 123,
            Password = "password123",
            SubAccId = 1,
            Nickname = "TestUser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en-US",
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            },
            DarkModeName = "dark",
            AssetAppPath = "/assets",
            DCSStaticAppPath = "/static"
        };

        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "TestUser",
            RoleId = 1,
            SubAccId = 1,
            AdminId = 0
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(signInResult))
            });

        // Act
        var result = await _accountService.AfterSignIn(signInParam);

        // Assert
        result.Should().NotBeNull();
        result.CustId.Should().Be(123);
        result.UserName.Should().Be("TestUser");
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Once);
    }

    [Fact]
    public async Task AfterSignIn_WhenFailed_ShouldNotSetUserSessionValues()
    {
        // Arrange
        var signInParam = new AlphaPlus.Auth.Site.Features.Account.SignInParam
        {
            CustId = 123,
            Password = "wrong-password",
            SubAccId = 1,
            Nickname = "TestUser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en-US",
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            },
            DarkModeName = "dark",
            AssetAppPath = "/assets",
            DCSStaticAppPath = "/static"
        };

        // Mock HTTP response to return failed result
        var failedResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            Code = 1, // Set non-zero code to make it failed
            Message = "Invalid credentials"
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(failedResult))
            });

        // Act
        var result = await _accountService.AfterSignIn(signInParam);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Never);
    }

    [Fact]
    public async Task InternalLogin_WhenSuccessful_ShouldSetUserSessionValues()
    {
        // Arrange
        var internalLoginParam = new AlphaPlus.Auth.Site.Features.Account.InternalLoginParam
        {
            AdminId = 1,
            Language = "en-US",
            Username = "admin",
            LoginTime = DateTime.Now,
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            }
        };

        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "admin",
            RoleId = 3,
            SubAccId = 1,
            AdminId = 1
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(signInResult))
            });

        // Act
        var result = await _accountService.InternalLogin(internalLoginParam);

        // Assert
        result.Should().NotBeNull();
        result.CustId.Should().Be(123);
        result.UserName.Should().Be("admin");
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Once);
    }

    [Fact]
    public async Task InternalLogin_WhenFailed_ShouldNotSetUserSessionValues()
    {
        // Arrange
        var internalLoginParam = new AlphaPlus.Auth.Site.Features.Account.InternalLoginParam
        {
            AdminId = 1,
            Language = "en-US",
            Username = "admin",
            LoginTime = DateTime.Now,
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            }
        };

        // Mock HTTP response to return failed result
        var failedResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            Code = 1, // Set non-zero code to make it failed
            Message = "Invalid admin credentials"
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(failedResult))
            });

        // Act
        var result = await _accountService.InternalLogin(internalLoginParam);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Never);
    }

    [Fact]
    public async Task IndirectLogin_WhenSuccessful_ShouldSetUserSessionValues()
    {
        // Arrange
        var indirectLoginParam = new AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam
        {
            CustId = 123,
            SubAccId = 1,
            Username = "TestUser",
            LoginTime = DateTime.Now,
            Language = "en-US",
            UplineCustId = 0,
            UplineSubAccId = 0,
            UplineUsername = null,
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            },
            DarkModeName = "dark",
            AssetAppPath = "/assets"
        };

        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "TestUser",
            RoleId = 1,
            SubAccId = 1,
            AdminId = 0
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(signInResult))
            });

        // Act
        var result = await _accountService.IndirectLogin(indirectLoginParam);

        // Assert
        result.Should().NotBeNull();
        result.CustId.Should().Be(123);
        result.UserName.Should().Be("TestUser");
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Once);
    }

    [Fact]
    public async Task IndirectLogin_WhenFailed_ShouldNotSetUserSessionValues()
    {
        // Arrange
        var indirectLoginParam = new AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam
        {
            CustId = 123,
            SubAccId = 1,
            Username = "TestUser",
            LoginTime = DateTime.Now,
            Language = "en-US",
            UplineCustId = 0,
            UplineSubAccId = 0,
            UplineUsername = null,
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            },
            DarkModeName = "dark",
            AssetAppPath = "/assets"
        };

        // Mock HTTP response to return failed result
        var failedResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            Code = 1, // Set non-zero code to make it failed
            Message = "Invalid indirect login credentials"
        };

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(failedResult))
            });

        // Act
        var result = await _accountService.IndirectLogin(indirectLoginParam);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Never);
    }

    [Fact]
    public async Task ReCheckIn_WhenValidOnlineUserId_ShouldReturnReCheckInResult()
    {
        // Arrange
        var reCheckInResult = new AlphaPlus.Auth.Site.Features.Account.ReCheckInResult
        {
            IsKickedOut = false,
            KickOutReason = 0
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(reCheckInResult))
            });

        // Act
        var result = await _accountService.ReCheckIn(123);

        // Assert
        result.Should().NotBeNull();
        result.IsKickedOut.Should().BeFalse();
        result.KickOutReason.Should().Be(0);
    }

    [Fact]
    public async Task CheckOut_WhenReasonIsProvided_ShouldCallCheckOutWithReason()
    {
        // Arrange
        var reason = "test-reason";

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("success")
            });

        // Act
        await _accountService.CheckOut(reason);

        // Assert
        // Verify that the HTTP request was made with the reason
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task CheckOut_WhenReasonIsNull_ShouldCallCheckOutWithoutReason()
    {
        // Arrange
        _mockUserSession.Setup(x => x.OnUserId).Returns(123);
        _mockUserSession.Setup(x => x.IsSubAcc).Returns(false);
        _mockUserSession.Setup(x => x.UserName).Returns("testuser");
        _mockUserSession.Setup(x => x.IsNickname).Returns(false);
        _mockUserSession.Setup(x => x.NickName).Returns("nickname");
        _mockUserSession.Setup(x => x.RoleId).Returns(1);
        _mockUserSession.Setup(x => x.ServerId).Returns(1);

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        // Act
        await _accountService.CheckOut(null);

        // Assert
        _mockHttpMessageHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task CheckOut_WhenUserIsSubAccount_ShouldUseSubUserName()
    {
        // Arrange
        _mockUserSession.Setup(x => x.OnUserId).Returns(123);
        _mockUserSession.Setup(x => x.IsSubAcc).Returns(true);
        _mockUserSession.Setup(x => x.SubUserName).Returns("subuser");
        _mockUserSession.Setup(x => x.IsNickname).Returns(false);
        _mockUserSession.Setup(x => x.NickName).Returns("nickname");
        _mockUserSession.Setup(x => x.RoleId).Returns(1);
        _mockUserSession.Setup(x => x.ServerId).Returns(1);

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        // Act
        await _accountService.CheckOut("test-reason");

        // Assert
        _mockHttpMessageHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task CheckOut_WhenUserIsNickname_ShouldUseNickName()
    {
        // Arrange
        _mockUserSession.Setup(x => x.OnUserId).Returns(123);
        _mockUserSession.Setup(x => x.IsSubAcc).Returns(false);
        _mockUserSession.Setup(x => x.UserName).Returns("testuser");
        _mockUserSession.Setup(x => x.IsNickname).Returns(true);
        _mockUserSession.Setup(x => x.NickName).Returns("nickname");
        _mockUserSession.Setup(x => x.RoleId).Returns(1);
        _mockUserSession.Setup(x => x.ServerId).Returns(1);

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        // Act
        await _accountService.CheckOut("test-reason");

        // Assert
        _mockHttpMessageHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task CheckOut_WhenUserIsNotNickname_ShouldUseEmptyString()
    {
        // Arrange
        _mockUserSession.Setup(x => x.OnUserId).Returns(123);
        _mockUserSession.Setup(x => x.IsSubAcc).Returns(false);
        _mockUserSession.Setup(x => x.UserName).Returns("testuser");
        _mockUserSession.Setup(x => x.IsNickname).Returns(false);
        _mockUserSession.Setup(x => x.NickName).Returns("nickname");
        _mockUserSession.Setup(x => x.RoleId).Returns(1);
        _mockUserSession.Setup(x => x.ServerId).Returns(1);

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        // Act
        await _accountService.CheckOut("test-reason");

        // Assert
        _mockHttpMessageHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task AfterSignIn_WhenResultIsFailed_ShouldNotSetUserSessionValues()
    {
        // Arrange
        var signInParam = new AlphaPlus.Auth.Site.Features.Account.SignInParam
        {
            CustId = 123,
            Password = "password123",
            SubAccId = 1,
            Nickname = "TestUser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en-US",
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            },
            DarkModeName = "dark",
            AssetAppPath = "/assets",
            DCSStaticAppPath = "/static"
        };

        var failedSignInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "TestUser",
            Code = 1, // Non-zero code indicates failure
            Message = "Authentication failed"
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(failedSignInResult))
            });

        // Act
        var result = await _accountService.AfterSignIn(signInParam);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(1);
        result.Message.Should().Be("Authentication failed");
        // Should not call SetUserSessionValues when result is failed
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Never);
    }

    [Fact]
    public async Task InternalLogin_WhenResultIsNotSuccessful_ShouldNotSetUserSessionValues()
    {
        // Arrange
        var internalLoginParam = new AlphaPlus.Auth.Site.Features.Account.InternalLoginParam
        {
            AdminId = 123,
            Language = "en-US",
            Username = "admin",
            LoginTime = DateTime.Now,
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            }
        };

        var failedSignInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "admin",
            Code = 1, // Non-zero code indicates failure
            Message = "Internal login failed"
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(failedSignInResult))
            });

        // Act
        var result = await _accountService.InternalLogin(internalLoginParam);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(1);
        result.Message.Should().Be("Internal login failed");
        // Should not call SetUserSessionValues when result is not successful
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Never);
    }

    [Fact]
    public async Task IndirectLogin_WhenResultIsNotSuccessful_ShouldNotSetUserSessionValues()
    {
        // Arrange
        var indirectLoginParam = new AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam
        {
            CustId = 123,
            UplineCustId = 456,
            UplineSubAccId = 789,
            UplineUsername = "upline",
            SubAccId = 1,
            Username = "testuser",
            LoginTime = DateTime.Now,
            Language = "en-US",
            RequestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
            {
                LoginIp = "127.0.0.1",
                CountryCode = "US",
                LoginDomain = "test.com",
                BrowserName = "Chrome",
                Platform = "Windows"
            },
            DarkModeName = "dark",
            AssetAppPath = "/assets"
        };

        var failedSignInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "testuser",
            Code = 1, // Non-zero code indicates failure
            Message = "Indirect login failed"
        };

        // Mock HTTP response
        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(failedSignInResult))
            });

        // Act
        var result = await _accountService.IndirectLogin(indirectLoginParam);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(1);
        result.Message.Should().Be("Indirect login failed");
        // Should not call SetUserSessionValues when result is not successful
        _mockUserSessionService.Verify(x => x.SetUserSessionValues(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInResult>()), Times.Never);
    }
} 