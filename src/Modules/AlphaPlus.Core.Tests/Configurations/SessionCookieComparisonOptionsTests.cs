using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Core.Tests.Configurations;

public class SessionCookieComparisonOptionsTests
{
    [Fact]
    public void SessionCookieComparisonOptions_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var options = new SessionCookieComparisonOptions();

        // Assert
        Assert.True(options.EnableSessionCookieComparison);
        Assert.Equal("Error", options.LogLevel);
        Assert.True(options.IncludeUserAgentInLog);
        Assert.True(options.IncludeRefererInLog);
        Assert.True(options.IncludeIpAddressInLog);
        Assert.NotNull(options.MonitoredPaths);
        Assert.Contains("/settings/creditbalance", options.MonitoredPaths);
        Assert.Contains("/reports/winlossdetail", options.MonitoredPaths);
    }

    [Fact]
    public void SessionCookieComparisonOptions_ShouldAllowCustomValues()
    {
        // Arrange
        var options = new SessionCookieComparisonOptions
        {
            EnableSessionCookieComparison = false,
            LogLevel = "Info",
            IncludeUserAgentInLog = false,
            IncludeRefererInLog = false,
            IncludeIpAddressInLog = false,
            MonitoredPaths = new[] { "/custom/path" }
        };

        // Assert
        Assert.False(options.EnableSessionCookieComparison);
        Assert.Equal("Info", options.LogLevel);
        Assert.False(options.IncludeUserAgentInLog);
        Assert.False(options.IncludeRefererInLog);
        Assert.False(options.IncludeIpAddressInLog);
        Assert.Single(options.MonitoredPaths);
        Assert.Contains("/custom/path", options.MonitoredPaths);
    }

    [Fact]
    public void SessionCookieComparisonOptions_SectionName_ShouldBeCorrect()
    {
        // Assert
        Assert.Equal("SessionCookieComparison", SessionCookieComparisonOptions.SectionName);
    }

    [Theory]
    [InlineData("Error")]
    [InlineData("Warning")]
    [InlineData("Info")]
    [InlineData("Debug")]
    public void SessionCookieComparisonOptions_LogLevel_ShouldAcceptValidValues(string logLevel)
    {
        // Arrange
        var options = new SessionCookieComparisonOptions
        {
            LogLevel = logLevel
        };

        // Assert
        Assert.Equal(logLevel, options.LogLevel);
    }

    [Fact]
    public void SessionCookieComparisonOptions_MonitoredPaths_ShouldBeCaseInsensitive()
    {
        // Arrange
        var options = new SessionCookieComparisonOptions
        {
            MonitoredPaths = new[] { "/UPPERCASE/PATH", "/lowercase/path", "/MixedCase/Path" }
        };

        // Assert
        Assert.Contains("/UPPERCASE/PATH", options.MonitoredPaths);
        Assert.Contains("/lowercase/path", options.MonitoredPaths);
        Assert.Contains("/MixedCase/Path", options.MonitoredPaths);
    }
} 