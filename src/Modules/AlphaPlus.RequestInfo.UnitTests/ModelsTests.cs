using AlphaPlus.RequestInfo;
using AlphaPlus.RequestInfo.Models;

namespace AlphaPlus.RequestInfo.UnitTests;

public class DeviceInfoTests
{
    [Fact]
    public void DeviceInfo_Properties_ShouldBeSettable()
    {
        // Arrange
        var deviceInfo = new DeviceInfo
        {
            IP = "192.168.1.1",
            Platform = "Windows 10",
            BrowserName = "Chrome 120.0.0.0"
        };

        // Act & Assert
        Assert.Equal("192.168.1.1", deviceInfo.IP);
        Assert.Equal("Windows 10", deviceInfo.Platform);
        Assert.Equal("Chrome 120.0.0.0", deviceInfo.BrowserName);
    }

    [Fact]
    public void DeviceInfo_Properties_ShouldBeNullable()
    {
        // Arrange
        var deviceInfo = new DeviceInfo();

        // Act & Assert
        Assert.Null(deviceInfo.IP);
        Assert.Null(deviceInfo.Platform);
        Assert.Null(deviceInfo.BrowserName);
    }
}

public class RequestInfoTests
{
    [Fact]
    public void RequestInfo_Properties_ShouldBeInitializable()
    {
        // Arrange
        var requestInfo = new Models.RequestInfo
        {
            LoginIp = "192.168.1.1",
            CountryCode = "US",
            LoginDomain = "example.com",
            BrowserName = "Chrome 120.0.0.0",
            Platform = "Windows 10"
        };

        // Act & Assert
        Assert.Equal("192.168.1.1", requestInfo.LoginIp);
        Assert.Equal("US", requestInfo.CountryCode);
        Assert.Equal("example.com", requestInfo.LoginDomain);
        Assert.Equal("Chrome 120.0.0.0", requestInfo.BrowserName);
        Assert.Equal("Windows 10", requestInfo.Platform);
    }

    [Fact]
    public void RequestInfo_Properties_ShouldBeNullable()
    {
        // Arrange
        var requestInfo = new Models.RequestInfo();

        // Act & Assert
        Assert.Null(requestInfo.LoginIp);
        Assert.Null(requestInfo.CountryCode);
        Assert.Null(requestInfo.LoginDomain);
        Assert.Null(requestInfo.BrowserName);
        Assert.Null(requestInfo.Platform);
    }

    [Fact]
    public void RequestInfo_BrowserName_ShouldBeSettable()
    {
        // Arrange
        var requestInfo = new Models.RequestInfo();
        var expectedBrowserName = "Firefox 119.0";

        // Act
        requestInfo.BrowserName = expectedBrowserName;

        // Assert
        Assert.Equal(expectedBrowserName, requestInfo.BrowserName);
    }
}