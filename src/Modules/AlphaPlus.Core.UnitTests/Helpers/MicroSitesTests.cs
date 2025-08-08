namespace AlphaPlus.Core.UnitTests.Helpers;

public class MicroSitesTests
{
    [Theory]
    [InlineData("~/", "/main/")]
    [InlineData("~/error", "/main/error")]
    public void MainUrl_Should_Map_To_Main(string virtualPath, string expected)
    {
        var result = MicroSites.MainUrl(virtualPath);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("~/", "/reports/")]
    [InlineData("~/winloss", "/reports/winloss")]
    public void ReportsUrl_Should_Map_To_Reports(string virtualPath, string expected)
    {
        var result = MicroSites.ReportsUrl(virtualPath);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("~/", "/tickets/")]
    [InlineData("~/betlists", "/tickets/betlists")]
    public void TicketUrl_Should_Map_To_Tickets(string virtualPath, string expected)
    {
        var result = MicroSites.TicketUrl(virtualPath);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetMicroSiteUrl_Should_Dispatch_By_Name()
    {
        var url = MicroSites.GetMicroSiteUrl("Main", "~/home");
        Assert.Equal("/main/home", url);
    }
}


