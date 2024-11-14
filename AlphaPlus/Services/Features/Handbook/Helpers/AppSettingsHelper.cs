using System;
using System.Configuration;
using System.Linq;

namespace AlphaPlus.Features.Handbook.API.Helpers
{
    public static class AppSettingsHelper
    {
        public static string ContentAPIEnvironment => Core.Configurations.ConfigurationManager.GetValue<string>("ContentAPI:Environment");
        public static string ContentAPIUrl => Core.Configurations.ConfigurationManager.GetValue<string>("ContentAPIUrl");
        public static string ContentAPIToken => Core.Configurations.ConfigurationManager.GetValue<string>("ContentAPIToken");

        public static string SiteLabel => Core.Configurations.ConfigurationManager.GetValue<string>("SiteLabel");
        public static string SiteLabel789y => Core.Configurations.ConfigurationManager.GetValue<string>("SiteLabel789y");
        public static bool IsSiteLabel789y => string.Equals(SiteLabel, SiteLabel789y);
        public static bool SupportVietnamese => IsSiteLabel789y;

        public static bool SupportedNewHandbook
        {
            get
            {
                var marketsSupportedNewHandbook = Core.Configurations.ConfigurationManager.GetValue<string>("MarketsSupportedNewHandbook");

                if (string.IsNullOrWhiteSpace(marketsSupportedNewHandbook))
                {
                    return false;
                }

                if (string.Equals("*", marketsSupportedNewHandbook, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var label = SiteLabel;
                return marketsSupportedNewHandbook
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Any(s => string.Equals(s, label, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}