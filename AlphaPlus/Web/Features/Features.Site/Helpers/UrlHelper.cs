namespace AlphaPlus.Features.Site.Messages.Helpers
{
    using System;
    using AlphaPlus.Core.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapExTotalBetsForecastPath;

        [ThreadStatic]
        private static Func<string, string> mapMessagesPath;

        [ThreadStatic]
        private static Func<string, string> mapOldMainPath;

        [ThreadStatic]
        private static Func<string, string> mapHowToUsePath;

        [ThreadStatic]
        private static Func<string, string> mapReportsPath;

        [ThreadStatic]
        private static Func<string, string> mapMemberInfoPath;

        [ThreadStatic]
        private static Func<string, string> mapMainPath;

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

        public static Func<string, string> MapOldMainPath
        {
            get
            {
                return mapOldMainPath ?? MicroSites.SettingUrl;
            }

            internal set
            {
                mapOldMainPath = value;
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
    }
}