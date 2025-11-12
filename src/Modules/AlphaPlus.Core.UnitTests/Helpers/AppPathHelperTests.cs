namespace AlphaPlus.Core.UnitTests.Helpers;

public class AppPathHelperTests
{
    [Theory]
    [InlineData("/main", "~/", "/main/")]
    [InlineData("/main", "~/error", "/main/error")]
    [InlineData("/features", "~/scripts/app.js", "/features/scripts/app.js")]
    [InlineData("/reports", "report", "/reports/report")]
    public void GetAppPath_Should_Combine_AppName_And_VirtualPath(string appName, string virtualPath, string expected)
    {
        var result = AppPathHelper.GetAppPath(appName, virtualPath);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetAppPath_Should_Throw_When_AppName_Is_Invalid(string appName)
    {
        var act = () => AppPathHelper.GetAppPath(appName, "~/");
        Assert.Throws<ArgumentNullException>(act);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetAppPath_Should_Throw_When_VirtualPath_Is_Invalid(string virtualPath)
    {
        var act = () => AppPathHelper.GetAppPath("/main", virtualPath);
        Assert.Throws<ArgumentNullException>(act);
    }
}


