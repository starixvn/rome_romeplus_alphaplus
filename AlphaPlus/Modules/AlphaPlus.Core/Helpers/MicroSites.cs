namespace AlphaPlus.Core.Helpers;

public static class MicroSites
{
    private static readonly IDictionary<string, Func<string, string>> microSiteUrls = new Dictionary<string, Func<string, string>>
    {
        { "Main", MainUrl},
        { "Reports", ReportsUrl},
        { "BetLists", BetListsUrl},
        { "MemberInfo", MemberInfoUrl },
        { "Message", MessageUrl},
        { "ExTotalBetsForecast", ExTotalBetsForecastUrl},
        { "HowToUse", HowToUseUrl},
        { "AccountInfo", AccountInfoUrl},
        { "ViewLog", ViewLogUrl },
        { "OldMain", OldMainUrl}
    };

    public static string GetMicroSiteUrl(string microSiteName, string virtualPath)
        => microSiteUrls[microSiteName](virtualPath);

    public static string MainUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/main", virtualPath);

    public static string ReportsUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/reports", virtualPath);

    public static string BetListsUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/tickets", virtualPath);

    public static string MemberInfoUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/settings", virtualPath);

    public static string MessageUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/features", virtualPath);

    public static string ExTotalBetsForecastUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/tickets", virtualPath);

    public static string HowToUseUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/features", virtualPath);

    public static string AccountInfoUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/account", virtualPath);

    public static string TransferUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/settings", virtualPath);

    public static string ViewLogUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/settings", virtualPath);

    public static string Settings(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/settings", virtualPath);

    public static string OldMainUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/settings", virtualPath);
}
