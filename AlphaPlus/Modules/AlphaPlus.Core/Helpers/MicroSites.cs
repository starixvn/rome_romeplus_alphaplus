namespace AlphaPlus.Core.Helpers;

public static class MicroSites
{
    private static readonly IDictionary<string, Func<string, string>> microSiteUrls = new Dictionary<string, Func<string, string>>
    {
        { "Main", MainUrl },
        { "Reports", ReportsUrl },
        { "BetLists", TicketUrl },
        { "MemberInfo", SettingUrl },
        { "Message", FeatureUrl },
        { "ExTotalBetsForecast", TicketUrl },
        { "HowToUse", FeatureUrl },
        { "AccountInfo", SettingUrl },
        { "ViewLog", SettingUrl },
        { "OldMain", SettingUrl },
        { "Settings", SettingUrl }
    };

    public static string GetMicroSiteUrl(string microSiteName, string virtualPath)
        => microSiteUrls[microSiteName](virtualPath);

    public static string MainUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/main", virtualPath);

    public static string ReportsUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/reports", virtualPath);

    public static string TicketUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/tickets", virtualPath);

    public static string FeatureUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/features", virtualPath);

    public static string SettingUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath("/settings", virtualPath);
}
