using Xunit;

namespace AlphaPlus.SecurityCode.UnitTests.Configurations;

public class SecurityCodeOptionsTests
{
    [Fact]
    public void SecurityCodeOptions_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var options = new SecurityCodeOptions();

        // Assert
        Assert.False(options.IsActive);
        Assert.Empty(options.Paths);
        Assert.Empty(options.BetListsPaths);
        Assert.Empty(options.StatementPaths);
        Assert.Empty(options.Sites);
        Assert.Equal("/main", options.SiteMainApp);
        Assert.Null(options.VerifyingSecurityCodePath);
        Assert.Null(options.ResetSecurityCodePath);
        Assert.Null(options.VerifyingOtpPath);
        Assert.False(options.IsSiteMain);
    }

    [Fact]
    public void SecurityCodeOptions_Properties_ShouldBeSettable()
    {
        // Arrange
        var options = new SecurityCodeOptions();
        var paths = new[] { "/api/secure", "/admin" };
        var betListsPaths = new[] { "/bets", "/history" };
        var statementPaths = new[] { "/statement", "/reports" };
        var sites = new[] { "site1", "site2" };

        // Act
        options.IsActive = true;
        options.Paths = paths;
        options.BetListsPaths = betListsPaths;
        options.StatementPaths = statementPaths;
        options.Sites = sites;
        options.SiteMainApp = "/custom";
        options.VerifyingSecurityCodePath = "/verify";
        options.ResetSecurityCodePath = "/reset";
        options.VerifyingOtpPath = "/otp";
        options.IsSiteMain = true;

        // Assert
        Assert.True(options.IsActive);
        Assert.Equal(paths, options.Paths);
        Assert.Equal(betListsPaths, options.BetListsPaths);
        Assert.Equal(statementPaths, options.StatementPaths);
        Assert.Equal(sites, options.Sites);
        Assert.Equal("/custom", options.SiteMainApp);
        Assert.Equal("/verify", options.VerifyingSecurityCodePath);
        Assert.Equal("/reset", options.ResetSecurityCodePath);
        Assert.Equal("/otp", options.VerifyingOtpPath);
        Assert.True(options.IsSiteMain);
    }

    [Fact]
    public void SecurityCodeOptions_SecurityCodeConstant_ShouldHaveCorrectValue()
    {
        // Assert
        Assert.Equal("SecurityCode", SecurityCodeOptions.SecurityCode);
    }
}
