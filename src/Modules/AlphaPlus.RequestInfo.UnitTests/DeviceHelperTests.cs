using System.Threading.Tasks;
using AlphaPlus.RequestInfo.Models;
using AlphaPlus.Utilities.IP;
using Microsoft.AspNetCore.Http;
using Moq;
using UAParser;

namespace AlphaPlus.RequestInfo.UnitTests;

public class DeviceHelperTests
{
    private readonly Mock<IPHelper> _mockIpHelper;
    private readonly Mock<IPService> _mockIpService;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<HttpContext> _mockHttpContext;
    private readonly Mock<HttpRequest> _mockHttpRequest;
    private readonly DeviceHelper _deviceHelper;

    public DeviceHelperTests()
    {
        _mockIpHelper = new Mock<IPHelper>();
        _mockIpService = new Mock<IPService>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockHttpContext = new Mock<HttpContext>();
        _mockHttpRequest = new Mock<HttpRequest>();

        _mockHttpContext.Setup(x => x.Request).Returns(_mockHttpRequest.Object);
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_mockHttpContext.Object);

        _deviceHelper = new DeviceHelper(_mockIpHelper.Object, _mockIpService.Object, _mockHttpContextAccessor.Object);
    }

    [Fact]
    public void GetUserAgent_ShouldReturnUserAgentFromHeaders()
    {
        // Arrange
        var expectedUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", expectedUserAgent } };
        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);

        // Act
        var result = _deviceHelper.GetUserAgent();

        // Assert
        Assert.Equal(expectedUserAgent, result);
    }

    [Fact]
    public void GetUserAgent_WhenUserAgentIsNull_ShouldReturnEmptyString()
    {
        // Arrange
        var headers = new HeaderDictionary();
        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);

        // Act
        var result = _deviceHelper.GetUserAgent();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetClientInfo_ShouldReturnParsedClientInfo()
    {
        // Arrange
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", userAgent } };
        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);

        // Act
        var result = _deviceHelper.GetClientInfo();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.UA);
        Assert.NotNull(result.OS);
        Assert.NotNull(result.Device);
    }

    [Fact]
    public void GetDeviceInfo_ShouldReturnDeviceInfoWithCorrectProperties()
    {
        // Arrange
        var expectedIp = "192.168.1.1";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", userAgent } };

        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);
        _mockIpHelper.Setup(x => x.GetClientIP()).Returns(expectedIp);

        // Act
        var result = _deviceHelper.GetDeviceInfo();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedIp, result.IP);
        Assert.NotNull(result.BrowserName);
        Assert.NotNull(result.Platform);
    }

    [Fact]
    public void GetDeviceInfo_WithBrowserSize_ShouldIncludeBrowserSizeInBrowserName()
    {
        // Arrange
        var expectedIp = "192.168.1.1";
        var browserSize = "1920x1080";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", userAgent } };

        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);
        _mockIpHelper.Setup(x => x.GetClientIP()).Returns(expectedIp);

        // Act
        var result = _deviceHelper.GetDeviceInfo(browserSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedIp, result.IP);
        Assert.Contains(browserSize, result.BrowserName);
    }

    [Fact]
    public void GetDeviceInfo_WithEmptyBrowserSize_ShouldNotIncludeBrowserSizeInBrowserName()
    {
        // Arrange
        var expectedIp = "192.168.1.1";
        var browserSize = "";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", userAgent } };

        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);
        _mockIpHelper.Setup(x => x.GetClientIP()).Returns(expectedIp);

        // Act
        var result = _deviceHelper.GetDeviceInfo(browserSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedIp, result.IP);
        Assert.DoesNotContain(", ", result.BrowserName);
    }

    [Fact]
    public async Task GetRequestInfo_ShouldReturnRequestInfoWithCorrectProperties()
    {
        // Arrange
        var expectedIp = "192.168.1.1";
        var expectedCountryCode = "US";
        var expectedHost = "example.com";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", userAgent } };

        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);
        _mockIpHelper.Setup(x => x.GetClientIP()).Returns(expectedIp);
        _mockIpService.Setup(x => x.GetCountryCode(expectedIp)).ReturnsAsync(expectedCountryCode);
        _mockHttpRequest.Setup(x => x.Host).Returns(new HostString(expectedHost));

        // Act
        var result = await _deviceHelper.GetRequestInfo();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedIp, result.LoginIp);
        Assert.Equal(expectedCountryCode, result.CountryCode);
        Assert.Equal(expectedHost, result.LoginDomain);
        Assert.NotNull(result.BrowserName);
        Assert.NotNull(result.Platform);
    }

    [Fact]
    public async Task GetRequestInfo_WhenIPServiceReturnsNull_ShouldHandleGracefully()
    {
        // Arrange
        var expectedIp = "192.168.1.1";
        var expectedHost = "example.com";
        var userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36";
        var headers = new HeaderDictionary { { "User-Agent", userAgent } };

        _mockHttpRequest.Setup(x => x.Headers).Returns(headers);
        _mockIpHelper.Setup(x => x.GetClientIP()).Returns(expectedIp);
        _mockIpService.Setup(x => x.GetCountryCode(expectedIp)).ReturnsAsync((string)null);
        _mockHttpRequest.Setup(x => x.Host).Returns(new HostString(expectedHost));

        // Act
        var result = await _deviceHelper.GetRequestInfo();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedIp, result.LoginIp);
        Assert.Null(result.CountryCode);
        Assert.Equal(expectedHost, result.LoginDomain);
    }
}