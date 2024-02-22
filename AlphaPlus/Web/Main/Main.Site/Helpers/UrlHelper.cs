using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Main.Site.Helpers
{
    public static class UrlHelper
    {
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

        public static Func<string, string> MapExTotalBetsForecastPath
        {
            get
            {
                return mapExTotalBetsForecastPath ?? MicroSites.ExTotalBetsForecastUrl;
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
                return mapMessagesPath ?? MicroSites.MessageUrl;
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
                return mapHowToUsePath ?? MicroSites.HowToUseUrl;
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
                return mapMemberInfoPath ?? MicroSites.MemberInfoUrl;
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
                return mapTransferPath ?? MicroSites.TransferUrl;
            }

            internal set
            {
                mapTransferPath = value;
            }
        }

        public static Func<string, string> MapBetListPath
        {
            get => mapBetListPath ?? MicroSites.BetListsUrl;
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
