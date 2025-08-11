using System.Net;
using System.Text.Json;
using AlphaPlus.Identity.API.Features.Auth.SiteDb;
using AlphaPlus.Identity.API.Utils;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace AlphaPlus.Identity.API.UnitTests.Features.Auth;

public class AuthenticateServiceTests
{
    private readonly Mock<IEncryption> _mockEncryption;
    private readonly Mock<IDynamicRepository> _mockDynamicRepository;
    private readonly Mock<IOptions<AppSettings>> _mockAppSettingsOptions;
    private readonly Mock<IMemoryCache> _mockMemoryCache;
    private readonly Mock<IAdminPermissionService> _mockAdminPermissionService;
    private readonly Mock<ISiteDBService> _mockSiteDBService;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly AuthenticateService _authenticateService;
    private readonly AppSettings _appSettings;

    public AuthenticateServiceTests()
    {
        _mockEncryption = new Mock<IEncryption>();
        _mockDynamicRepository = new Mock<IDynamicRepository>();
        _mockAppSettingsOptions = new Mock<IOptions<AppSettings>>();
        _mockMemoryCache = new Mock<IMemoryCache>();
        _mockAdminPermissionService = new Mock<IAdminPermissionService>();
        _mockSiteDBService = new Mock<ISiteDBService>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _appSettings = new AppSettings
        {
            SettingStatus = 1,
            AllowedListSuperAccounts = "123,456,789",
            AutoFollowBetMasterId = 100,
            IsTest = false,
            PasswordService = new PasswordService
            {
                IsEnabled = true,
                Uri = "http://test.com/api/"
            }
        };

        _mockAppSettingsOptions.Setup(x => x.Value).Returns(_appSettings);

        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

        _authenticateService = new AuthenticateService(
            _mockEncryption.Object,
            _mockDynamicRepository.Object,
            _mockAppSettingsOptions.Object,
            _mockMemoryCache.Object,
            _mockAdminPermissionService.Object,
            _mockSiteDBService.Object,
            _httpClient);
    }

    [Fact]
    public async Task LoginClaimDownline_WithValidCredentials_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            CustId = 123,
            Password = "testpass",
            SubAccId = 0,
            Nickname = "testuser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en",
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "testsite",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        var loginCommand = new LoginClaimDownlineCommand
        {
            CustId = 123,
            UserName = "testuser",
            SubAccId = 0,
            RoleId = 2,
            Site = "testsite",
            CurrencyId = 1,
            PwdExpiry = DateTime.Now.AddDays(30)
        };

        _mockEncryption.Setup(x => x.CfsEnCode(It.IsAny<string>())).Returns("hashedpassword");
        _mockDynamicRepository.Setup(x => x.ExecuteAsync(It.IsAny<LoginClaimDownlineCommand>(), "", default))
            .Callback<LoginClaimDownlineCommand, string, CancellationToken>((cmd, _, _) =>
            {
                cmd.ErrCode = 0;
                cmd.UserName = loginCommand.UserName;
                cmd.RoleId = loginCommand.RoleId;
                cmd.Site = loginCommand.Site;
                cmd.CurrencyId = loginCommand.CurrencyId;
                cmd.PwdExpiry = loginCommand.PwdExpiry;
            })
            .Returns(Task.FromResult(0));

        SetupMockCustomerStatus();
        SetupMockSiteDBConfig();
        SetupMockSecurityCodeFunction();
        SetupMockJackpotProducts();
        SetupMockAutoFollowBetDummyAgent();
        SetupMockProductGroupIds();

        // Act
        var result = await _authenticateService.LoginClaimDownline(request);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(0);
        result.CustId.Should().Be(123);
        result.UserName.Should().Be("testuser");
        result.Site.Should().Be("testsite");
        result.CurrencyId.Should().Be(1);
    }

    [Fact]
    public async Task LoginClaimDownline_WithInvalidCredentials_ShouldReturnErrorResponse()
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            CustId = 123,
            Password = "invalidpass",
            SubAccId = 0,
            Nickname = "invaliduser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en",
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "testsite",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        _mockEncryption.Setup(x => x.CfsEnCode(It.IsAny<string>())).Returns("hashedpassword");
        _mockDynamicRepository.Setup(x => x.ExecuteAsync(It.IsAny<LoginClaimDownlineCommand>(), "", default))
            .Callback<LoginClaimDownlineCommand, string, CancellationToken>((cmd, _, _) =>
            {
                cmd.ErrCode = 1;
                cmd.ErrMsg = "Invalid credentials";
            })
            .Returns(Task.FromResult(0));

        // Act
        var result = await _authenticateService.LoginClaimDownline(request);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(1);
        result.Message.Should().Be("Invalid credentials");
    }

    [Fact]
    public async Task InternalLogin_WithValidCredentials_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new InternalLoginRequest
        {
            Username = "internaluser",
            AdminId = 101,
            BrowserName = "Chrome",
            CountryCode = "US",
            Language = "en",
            LoginDomain = "testsite",
            LoginIp = "127.0.0.1",
            LoginTime = DateTime.Now,
            Platform = "Windows"
        };

        var loginCriteria = new InternalLoginCriteria
        {
            CustId = 789,
            AdminId = 101,
            RoleId = 3,
            Site = "testsite",
            CurrencyId = 2
        };

        _mockDynamicRepository.Setup(x => x.ExecuteAsync(It.IsAny<InternalLoginCriteria>(), "", default))
            .Callback<InternalLoginCriteria, string, CancellationToken>((crit, _, _) =>
            {
                crit.ErrCode = 0;
                crit.CustId = loginCriteria.CustId;
                crit.AdminId = loginCriteria.AdminId;
                crit.RoleId = loginCriteria.RoleId;
                crit.Site = loginCriteria.Site;
                crit.CurrencyId = loginCriteria.CurrencyId;
            })
            .Returns(Task.FromResult(0));

        SetupMockCustomerStatus();
        SetupMockSiteDBConfig();
        SetupMockSecurityCodeFunction();
        SetupMockJackpotProducts();
        SetupMockAutoFollowBetDummyAgent();
        SetupMockProductGroupIds();
        SetupMockAdminPermission();

        // Act
        var result = await _authenticateService.InternalLogin(request);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(0);
        result.CustId.Should().Be(789);
        result.AdminId.Should().Be(101);
        result.Site.Should().Be("testsite");
        result.CurrencyId.Should().Be(2);
    }

    [Fact]
    public async Task IndirectLogin_WithValidCredentials_ShouldReturnSuccessResponse()
    {
        // Arrange
        var request = new IndirectLoginRequest
        {
            CustId = 999,
            Username = "indirectuser",
            SubAccId = 303,
            UplineUsername = "uplineuser",
            UplineCustId = 888,
            UplineSubAccId = 0,
            LoginTime = DateTime.Now,
            Language = "en",
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "testsite",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        var loginCriteria = new IndirectLoginCriteria
        {
            CustId = 999,
            SubAccId = 303,
            RoleId = 2,
            Site = "testsite",
            CurrencyId = 1,
            PwdExpiry = DateTime.Now.AddDays(60)
        };

        _mockDynamicRepository.Setup(x => x.ExecuteAsync(It.IsAny<IndirectLoginCriteria>(), "", default))
            .Callback<IndirectLoginCriteria, string, CancellationToken>((crit, _, _) =>
            {
                crit.ErrCode = 0;
                crit.CustId = loginCriteria.CustId;
                crit.SubAccId = loginCriteria.SubAccId;
                crit.RoleId = loginCriteria.RoleId;
                crit.Site = loginCriteria.Site;
                crit.CurrencyId = loginCriteria.CurrencyId;
                crit.PwdExpiry = loginCriteria.PwdExpiry;
            })
            .Returns(Task.FromResult(0));

        SetupMockCustomerStatus();
        SetupMockSiteDBConfig();
        SetupMockSecurityCodeFunction();
        SetupMockJackpotProducts();
        SetupMockAutoFollowBetDummyAgent();
        SetupMockProductGroupIds();

        // Act
        var result = await _authenticateService.IndirectLogin(request);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(0);
        result.CustId.Should().Be(999);
        result.SubAccId.Should().Be(303);
        result.Site.Should().Be("testsite");
        result.CurrencyId.Should().Be(1);
    }

    [Fact]
    public async Task CheckIn_WithValidRequest_ShouldReturnOnlineUserId()
    {
        // Arrange
        var request = new CheckInRequest
        {
            CustId = 123,
            SubAccId = 0,
            SessionId = "session123",
            ServerId = 1,
            IP = "127.0.0.1"
        };

        var expectedOnlineUserId = 789;

        _mockDynamicRepository.Setup(x => x.ExecuteAsync(It.IsAny<CheckInCommand>(), "", default))
            .Callback<CheckInCommand, string, CancellationToken>((command, _, _) =>
            {
                command.OnlineUserId = expectedOnlineUserId;
            })
            .Returns(Task.FromResult(0));

        // Act
        var result = await _authenticateService.CheckIn(request);

        // Assert
        result.Should().Be(expectedOnlineUserId);
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(
            It.Is<CheckInCommand>(cmd =>
                cmd.CustId == request.CustId &&
                cmd.SubAccId == request.SubAccId &&
                cmd.NetSessionId == request.SessionId &&
                cmd.ServerId == request.ServerId &&
                cmd.IP == request.IP), "", default), Times.Once);
    }

    [Fact]
    public async Task CheckOut_WithValidRequest_ShouldCallRepository()
    {
        // Arrange
        var request = new CheckOutRequest
        {
            OnlineUserId = 123,
            Username = "testuser",
            RoleId = 2,
            ServerId = 1,
            Nickname = "testnick",
            Reason = "logout"
        };

        // Act
        await _authenticateService.CheckOut(request);

        // Assert
        _mockDynamicRepository.Verify(x => x.ExecuteAsync(
            It.Is<CheckOutCommand>(cmd =>
                cmd.OnlineUserId == request.OnlineUserId &&
                cmd.Username == request.Username &&
                cmd.RoleId == request.RoleId &&
                cmd.ServerId == request.ServerId &&
                cmd.Nickname == request.Nickname &&
                cmd.Reason == request.Reason), "", default), Times.Once);
    }

    [Fact]
    public async Task ReCheckIn_WithValidOnlineUserId_ShouldReturnCorrectResponse()
    {
        // Arrange
        var onlineUserId = 123;
        var expectedIsKickedOut = true;
        var expectedKickOutReason = (byte)1;

        _mockDynamicRepository
            .Setup(x => x.ExecuteAsync(It.IsAny<ReCheckInCommand>(), "", default))
            .Callback<ReCheckInCommand, string, CancellationToken>((command, _, _) =>
            {
                command.IsKickedOut = expectedIsKickedOut;
                command.KickOutReason = expectedKickOutReason;
            })
            .Returns(Task.FromResult(0));

        // Act
        var result = await _authenticateService.ReCheckIn(onlineUserId);

        // Assert
        result.Should().NotBeNull();
        result.IsKickedOut.Should().Be(expectedIsKickedOut);
        result.KickOutReason.Should().Be(expectedKickOutReason);

        _mockDynamicRepository.Verify(x => x.ExecuteAsync(It.IsAny<ReCheckInCommand>(), "", default), Times.Once);
    }

    [Theory]
    [InlineData("123,456,789", true)]
    [InlineData("123,456,789", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public async Task LoginClaimDownline_WithSuperAccount_ShouldHandleCorrectly(string allowedAccounts, bool isSuperAccount)
    {
        // Arrange
        var request = new AuthenticateRequest
        {
            CustId = 123,
            Password = "superpass",
            SubAccId = 0,
            Nickname = "superuser",
            IsNickname = false,
            LoginTime = DateTime.Now,
            Language = "en",
            LoginIp = "127.0.0.1",
            CountryCode = "US",
            LoginDomain = "testsite",
            BrowserName = "Chrome",
            Platform = "Windows"
        };

        _appSettings.AllowedListSuperAccounts = allowedAccounts;

        var loginCommand = new LoginClaimDownlineCommand
        {
            CustId = 123,
            UserName = "superuser",
            SubAccId = 0,
            RoleId = 4, // Super role
            Site = "testsite",
            CurrencyId = 1
        };

        _mockEncryption.Setup(x => x.CfsEnCode(It.IsAny<string>())).Returns("hashedpassword");
        _mockDynamicRepository.Setup(x => x.ExecuteAsync(It.IsAny<LoginClaimDownlineCommand>(), "", default))
            .Callback<LoginClaimDownlineCommand, string, CancellationToken>((cmd, _, _) =>
            {
                cmd.ErrCode = 0;
                cmd.UserName = loginCommand.UserName;
                cmd.RoleId = loginCommand.RoleId;
                cmd.Site = loginCommand.Site;
                cmd.CurrencyId = loginCommand.CurrencyId;
            })
            .Returns(Task.FromResult(0));

        SetupMockCustomerStatus();
        SetupMockSiteDBConfig();
        SetupMockSecurityCodeFunction();
        SetupMockJackpotProducts();
        SetupMockAutoFollowBetDummyAgent();
        SetupMockProductGroupIds();

        // Act
        var result = await _authenticateService.LoginClaimDownline(request);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be(0);
        result.CustId.Should().Be(123);
    }

    private void SetupMockCustomerStatus()
    {
        var customerStatus = new Dictionary<string, string>
        {
            { "mrecommend", "100" },
            { "arecommend", "200" },
            { "srecommend", "300" }
        };

        _mockDynamicRepository.Setup(x => x.FetchAsync<object>(It.IsAny<ICriteria>(), "", default))
            .ReturnsAsync(new List<object> { customerStatus });
    }

    private void SetupMockSiteDBConfig()
    {
        var siteDBConfig = new SiteDBConfig
        {
            // Add properties as needed
        };

        _mockSiteDBService.Setup(x => x.GetSiteDBConfig(It.IsAny<int>()))
            .ReturnsAsync(siteDBConfig);
    }

    private void SetupMockSecurityCodeFunction()
    {
        var securityCodeFunction = new SecurityCodeFunction
        {
            BetList = true,
            Statement = false
        };

        _mockDynamicRepository.Setup(x => x.FetchAsync<SecurityCodeFunction>(It.IsAny<ICriteria>(), "", default))
            .ReturnsAsync(new List<SecurityCodeFunction> { securityCodeFunction });
    }

    private void SetupMockJackpotProducts()
    {
        var jackpotOffered = new JackpotOffered(
            new List<JackpotOfferedBySite>
            {
                new() { Site = "testsite", ProductId = 1 },
                new() { Site = "testsite", ProductId = 2 }
            },
            new List<JackpotOfferedByCurrency>
            {
                new() { Currency = 1, ProductId = 1 },
                new() { Currency = 1, ProductId = 2 }
            });

        _mockDynamicRepository.Setup(x => x.FetchAsync<JackpotOffered>(It.IsAny<ICriteria>(), "", default))
            .ReturnsAsync(new List<JackpotOffered> { jackpotOffered });
    }

    private void SetupMockAutoFollowBetDummyAgent()
    {
        _mockDynamicRepository.Setup(x => x.FetchAsync<bool>(It.IsAny<ICriteria>(), "", default))
            .ReturnsAsync(new List<bool> { true });
    }

    private void SetupMockProductGroupIds()
    {
        var productSettings = new List<ProductSetting>
        {
            new() { ProductId = 1, GroupId = 10, IsHidden = false },
            new() { ProductId = 2, GroupId = 20, IsHidden = false }
        };

        _mockDynamicRepository.Setup(x => x.FetchAsync<ProductSetting>(It.IsAny<ICriteria>(), "", default))
            .ReturnsAsync(productSettings);
    }

    private void SetupMockAdminPermission()
    {
        _mockAdminPermissionService.Setup(x => x.HasUpdateSubAccountPermission(It.IsAny<int>()))
            .ReturnsAsync(true);
    }
}

