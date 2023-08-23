namespace Alpha.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using AlphaPlus.Configurations;
    using Fanex.Logging.Extensions.Web;

    public static class MicroSites
    {
        private static readonly IDictionary<string, Func<string, string>> microSiteUrls =
          new Dictionary<string, Func<string, string>>
          {
                { "OldMain", OldMainUrl },
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
        {
            try
            {
                return microSiteUrls[microSiteName](virtualPath);
            }
            catch (Exception exception)
            {
                ExceptionHandler.Handle(exception, TraceEventType.Error);
                return string.Empty;
            }
        }

        public static string OldMainUrl(string virtualPath = "~/")
            => AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, virtualPath);

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
}
