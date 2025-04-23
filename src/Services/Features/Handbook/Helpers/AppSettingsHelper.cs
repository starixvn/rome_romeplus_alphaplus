using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Features.Handbook.API.Helpers
{
    public static class AppSettingsHelper
    {
        public static string ContentAPIEnvironment => ConfigurationManager.GetValue<string>("ContentAPI:Environment");
        public static string ContentAPIUrl => ConfigurationManager.GetValue<string>("ContentAPIUrl");
        public static string ContentAPIToken => ConfigurationManager.GetValue<string>("ContentAPIToken");

        public static string SiteLabel => ConfigurationManager.GetValue<string>("SiteLabel");
        public static string SiteLabel789y => ConfigurationManager.GetValue<string>("SiteLabel789y");
        public static bool IsSiteLabel789y => string.Equals(SiteLabel, SiteLabel789y);
        public static bool SupportVietnamese => IsSiteLabel789y;
    }
}