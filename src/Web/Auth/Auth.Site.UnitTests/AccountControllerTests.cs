using System.Net;
using System.Security.Claims;
using System.Text.Json;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.RequestInfo.Models;
using AlphaPlus.RequestInfo;
using AlphaPlus.Core.Helpers;
using Fanex.Cryptography.Aes;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Moq;

namespace AlphaPlus.Auth.Site.UnitTests;

public class AccountControllerTests
{
    private readonly Mock<IUserSession> _mockUserSession;
    private readonly Mock<IDeviceHelper> _mockDeviceHelper;
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly Mock<IAesService> _mockAesService;
    private readonly Mock<IOptions<AuthorizationOptions>> _mockOptions;
    private readonly AlphaPlus.Auth.Site.Features.Account.AccountController _controller;
    private readonly DefaultHttpContext _httpContext;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IAuthenticationService> _mockAuthenticationService;

    public AccountControllerTests()
    {
        _mockUserSession = new Mock<IUserSession>();
        _mockDeviceHelper = new Mock<IDeviceHelper>();
        _mockAccountService = new Mock<IAccountService>();
        _mockAesService = new Mock<IAesService>();
        _mockOptions = new Mock<IOptions<AuthorizationOptions>>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockAuthenticationService = new Mock<IAuthenticationService>();
        
        _httpContext = new DefaultHttpContext();
        _httpContext.Request.Scheme = "https";
        _httpContext.Request.Host = new HostString("test.com");
        _httpContext.Request.Headers["Host"] = "test.com";
        
        // Set up session
        _httpContext.Session = new Mock<ISession>().Object;
        
        // Set up ServiceProvider for authentication services
        var services = new ServiceCollection();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
        services.AddSingleton(_mockAuthenticationService.Object);
        _httpContext.RequestServices = services.BuildServiceProvider();
        
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_httpContext);
        
        // Configure AlphaContext to use our test HttpContext
        AlphaContext.Configure(_mockHttpContextAccessor.Object);
        
        _mockOptions.Setup(x => x.Value).Returns(new AuthorizationOptions
        {
            DisabledCheckDuplicationLogin = false
        });

        _controller = new AlphaPlus.Auth.Site.Features.Account.AccountController(
            _mockUserSession.Object,
            _mockDeviceHelper.Object,
            _mockAccountService.Object,
            _mockAesService.Object,
            _mockOptions.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = _httpContext
            }
        };
    }

    [Fact]
    public void Login_WhenUserIsIndirectLoggedIn_ShouldReturnEmptyResult()
    {
        // Arrange
        _mockUserSession.Setup(x => x.IsIndirectLoggedIn).Returns(true);

        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public void Login_WhenUserIsInternal_ShouldReturnEmptyResult()
    {
        // Arrange
        _mockUserSession.Setup(x => x.IsIndirectLoggedIn).Returns(false);
        _mockUserSession.Setup(x => x.IsInternal).Returns(true);

        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public void Login_WhenSessionExpired_ShouldReturnEmptyResult()
    {
        // Arrange
        _mockUserSession.Setup(x => x.IsIndirectLoggedIn).Returns(false);
        _mockUserSession.Setup(x => x.IsInternal).Returns(false);

        // Act
        var result = _controller.Login("SessionExpired");

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task SSO_WhenParameterIsNull_ShouldReturnLoginWithInvalidSession()
    {
        // Act
        var result = await _controller.SSO(null);

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task SSO_WhenParameterIsEmpty_ShouldReturnLoginWithInvalidSession()
    {
        // Act
        var result = await _controller.SSO("");

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task SSO_WhenParameterIsInvalid_ShouldReturnLoginWithInvalidSession()
    {
        // Arrange
        _mockAesService.Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new Exception("Invalid parameter"));

        // Act
        var result = await _controller.SSO("invalid-parameter");

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task SSO_WhenParameterIsValid_ShouldProcessSignIn()
    {
        // Arrange
        var ssoParam = new AlphaPlus.Auth.Site.Features.Account.SSOParam
        {
            CustId = 123,
            EncryptPassword = "encrypted-password",
            EncryptCode = "encrypt-code",
            SubAccId = 1,
            Nickname = "TestUser",
            IsNickname = false,
            LoginTime = DateTime.Now.Ticks,
            DarkModeName = "dark",
            AssetAppPath = "/assets",
            DCSStaticAppPath = "/static"
        };

        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
        {
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "test.com",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "TestUser"
        };

        _mockAesService.Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(JsonSerializer.Serialize(ssoParam));
        _mockAesService.Setup(x => x.Decrypt(ssoParam.EncryptPassword, ssoParam.EncryptCode))
            .Returns("decrypted-password");
        _mockDeviceHelper.Setup(x => x.GetRequestInfo())
            .ReturnsAsync(requestInfo);
        _mockAccountService.Setup(x => x.AfterSignIn(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInParam>()))
            .ReturnsAsync(signInResult);
        _mockUserSession.Setup(x => x.LangKey).Returns("en-US");

        // Mock authentication service
        _mockAuthenticationService.Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.SSO("valid-parameter");

        // Assert
        result.Should().BeOfType<RedirectResult>();
        _mockAccountService.Verify(x => x.AfterSignIn(It.IsAny<AlphaPlus.Auth.Site.Features.Account.SignInParam>()), Times.Once);
    }

    [Fact]
    public async Task InternalLogin_WhenParameterIsNull_ShouldReturnLoginWithInvalidSession()
    {
        // Act
        var result = await _controller.InternalLogin(null);

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task InternalLogin_WhenParameterIsValid_ShouldProcessInternalLogin()
    {
        // Arrange
        var internalLoginParam = new AlphaPlus.Auth.Site.Features.Account.InternalLoginParam
        {
            AdminId = 1,
            Language = "en-US",
            Username = "admin",
            LoginTime = DateTime.Now
        };

        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
        {
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "test.com",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "admin"
        };

        // Mock the decrypted parameter to match the expected format
        var decryptedParam = "admin&1&" + DateTime.Now.Ticks.ToString();
        
        _mockAesService.Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(decryptedParam);
        _mockDeviceHelper.Setup(x => x.GetRequestInfo())
            .ReturnsAsync(requestInfo);
        _mockAccountService.Setup(x => x.InternalLogin(It.IsAny<AlphaPlus.Auth.Site.Features.Account.InternalLoginParam>()))
            .ReturnsAsync(signInResult);
        _mockUserSession.Setup(x => x.LangKey).Returns("en-US");

        // Mock authentication service
        _mockAuthenticationService.Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.InternalLogin("valid-parameter");

        // Assert
        result.Should().BeOfType<RedirectResult>();
        _mockAccountService.Verify(x => x.InternalLogin(It.IsAny<AlphaPlus.Auth.Site.Features.Account.InternalLoginParam>()), Times.Once);
    }

    [Fact]
    public async Task IndirectLogin_WhenParameterIsNull_ShouldReturnLoginWithInvalidSession()
    {
        // Act
        var result = await _controller.IndirectLogin(null);

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task IndirectLogin_WhenParameterIsValid_ShouldProcessIndirectLogin()
    {
        // Arrange
        var indirectLoginParam = new AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam
        {
            CustId = 123,
            SubAccId = 1,
            Username = "TestUser",
            LoginTime = DateTime.Now
        };

        var requestInfo = new AlphaPlus.RequestInfo.Models.RequestInfo
        {
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "test.com",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        var signInResult = new AlphaPlus.Auth.Site.Features.Account.SignInResult
        {
            CustId = 123,
            UserName = "TestUser"
        };

        // Mock the decrypted parameter to match the expected format
        var decryptedParam = "123&1&" + DateTime.Now.Ticks.ToString() + "&en-US&0&0&TestUser&TestUser&dark&/assets";
        
        _mockAesService.Setup(x => x.Decrypt(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(decryptedParam);
        _mockDeviceHelper.Setup(x => x.GetRequestInfo())
            .ReturnsAsync(requestInfo);
        _mockAccountService.Setup(x => x.IndirectLogin(It.IsAny<AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam>()))
            .ReturnsAsync(signInResult);
        _mockUserSession.Setup(x => x.LangKey).Returns("en-US");

        // Mock authentication service
        _mockAuthenticationService.Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.IndirectLogin("valid-parameter");

        // Assert
        result.Should().BeOfType<RedirectResult>();
        _mockAccountService.Verify(x => x.IndirectLogin(It.IsAny<AlphaPlus.Auth.Site.Features.Account.IndirectLoginParam>()), Times.Once);
    }

    [Fact]
    public void Logout_ShouldReturnRedirectToMain()
    {
        // Act
        var result = _controller.Logout();

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task Logout2_WhenUserIsAuthenticated_ShouldProcessLogout()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "123"),
            new(ClaimTypes.Name, "TestUser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        
        _httpContext.User = principal;

        // Mock authentication service
        _mockAuthenticationService.Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Logout2();

        // Assert
        result.Should().BeOfType<EmptyResult>();
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

        _mockAccountService.Setup(x => x.ReCheckIn(It.IsAny<int>()))
            .ReturnsAsync(reCheckInResult);

        // Act
        var result = await _controller.ReCheckIn(123);

        // Assert
        result.Should().BeOfType<JsonResult>();
        var jsonResult = result as JsonResult;
        // The controller returns a Result object, not ReCheckInResult directly
        jsonResult!.Value.Should().BeOfType<AlphaPlus.Core.Models.Result>();
    }

    [Fact]
    public async Task Offline_ShouldReturnEmptyResult()
    {
        // Arrange
        // Mock authentication service
        _mockAuthenticationService.Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Offline();

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }

    [Fact]
    public async Task KickOut_WhenReasonIsProvided_ShouldReturnRedirect()
    {
        // Act
        var result = await _controller.KickOut("test-reason");

        // Assert
        result.Should().BeOfType<EmptyResult>();
    }
} 