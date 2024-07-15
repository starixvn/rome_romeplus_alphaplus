using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Main.Site.Helpers
{
    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapSettingsPath;

        [ThreadStatic]
        private static Func<string, string> mapExTotalBetsForecastPath;

        [ThreadStatic]
        private static Func<string, string> mapMessagesPath;

        [ThreadStatic]
        private static Func<string, string> mapHowToUsePath;

        [ThreadStatic]
        private static Func<string, string> mapReportsPath;

        [ThreadStatic]
        private static Func<string, string> mapMemberInfoPath;

        [ThreadStatic]
        private static Func<string, string> mapAccountInfoPath;

        [ThreadStatic]
        private static Func<string, string> mapMainPath;

        [ThreadStatic]
        private static Func<string, string> mapBetListPath;

        [ThreadStatic]
        private static Func<string, string> mapTransferPath;

        public static Func<string, string> MapSettingsPath
        {
            get => mapSettingsPath ?? MicroSites.SettingUrl;
            internal set => mapSettingsPath = value;
        }

        public static Func<string, string> MapExTotalBetsForecastPath
        {
            get
            {
                return mapExTotalBetsForecastPath ?? MicroSites.TicketUrl;
            }

            internal set
            {
                mapExTotalBetsForecastPath = value;
            }
        }

        public static Func<string, string> MapMessagesPath
        {
            get
            {
                return mapMessagesPath ?? MicroSites.FeatureUrl;
            }

            internal set
            {
                mapMessagesPath = value;
            }
        }

        public static Func<string, string> MapHowToUsePath
        {
            get
            {
                return mapHowToUsePath ?? MicroSites.FeatureUrl;
            }

            internal set
            {
                mapHowToUsePath = value;
            }
        }

        public static Func<string, string> MapReportsPath
        {
            get
            {
                return mapReportsPath ?? MicroSites.ReportsUrl;
            }

            internal set
            {
                mapReportsPath = value;
            }
        }

        public static Func<string, string> MapMemberInfoPath
        {
            get
            {
                return mapMemberInfoPath ?? MicroSites.SettingUrl;
            }

            internal set
            {
                mapMemberInfoPath = value;
            }
        }

        public static Func<string, string> MapAccountInfoPath
        {
            get
            {
                return mapAccountInfoPath ?? MicroSites.AccountInfoUrl;
            }

            internal set
            {
                mapAccountInfoPath = value;
            }
        }

        public static Func<string, string> MapMainPath
        {
            get
            {
                return mapMainPath ?? MicroSites.MainUrl;
            }

            internal set
            {
                mapMainPath = value;
            }
        }

        public static Func<string, string> MapTransferPath
        {
            get
            {
                return mapTransferPath ?? MicroSites.SettingUrl;
            }

            internal set
            {
                mapTransferPath = value;
            }
        }

        public static Func<string, string> MapBetListPath
        {
            get => mapBetListPath ?? MicroSites.TicketUrl;
            internal set
            {
                mapBetListPath = value;
            }
        }

        public static string MapResultPath()
        {
            return AppPathHelper.GetAppPath("site-results", "~/");
        }
    }
}
