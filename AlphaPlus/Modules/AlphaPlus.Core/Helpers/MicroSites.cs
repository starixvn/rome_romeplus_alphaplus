using AlphaPlus.Core.Configurations;

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
        { "ViewLog", ViewLogUrl }
    };

    public static string GetMicroSiteUrl(string microSiteName, string virtualPath)
        => microSiteUrls[microSiteName](virtualPath);

    public static string MainUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.MainAppPath, virtualPath);

    public static string ReportsUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.ReportsAppPath, virtualPath);

    public static string BetListsUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.BetListsAppPath, virtualPath);

    public static string MemberInfoUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.MemberInfoAppPath, virtualPath);

    public static string MessageUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.MessageAppPath, virtualPath);

    public static string ExTotalBetsForecastUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.ExTotalBetsForecastAppPath, virtualPath);

    public static string HowToUseUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.HowToUseAppPath, virtualPath);

    public static string AccountInfoUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.AccountInfoAppPath, virtualPath);

    public static string TransferUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.TransferAppPath, virtualPath);

    public static string ViewLogUrl(string virtualPath = "~/")
        => AppPathHelper.GetAppPath(AppSettings.ViewLogsAppPath, virtualPath);
}
