using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Main.Site
{
    using AlphaPlus.Core.Configurations;

    public static class AppSettings
    {
        public static string[] DevicesUseCustomScrollbar => GetValue("DevicesUseCustomScrollbar", string.Empty).Split(',');

        public static bool IsDisabledContactUsLink => GetValue<bool>("IsDisabledContactUsLink");

        public static bool IsMBCSite => !IsSiteLabel789y;

        public static bool IsSiteLabel789y => SiteLabel.Equals("789y");

        public static bool DisabledVietnamese => IsMBCSite;

        public static bool ShowHelpLink => IsMBCSite;

        public static int[] TrackingConnectionCustIds => GetSectionValue<int[]>("TrackingConnectionCustIds", []);

        public static bool BannerIsHide => GetValue<bool>("Banner:IsHide");

        public static string BannerEnvironment => GetValue<string>("Banner:Environment");

        public static bool BannerIsHideMobileSabaApp => GetValue<bool>("Banner:IsHideMobileSabaApp");

        public static string SiteLabel => GetValue<string>("SiteLabel");

        public static string SiteLabel789y => GetValue<string>("SiteLabel789y");

        public static bool EnableUMAnnouncementPopup => GetValue<bool>("EnableUMAnnouncementPopup");

        public static int UMAnnouncementPopupServerCacheInMinutes => GetValue<int>("UMAnnouncementPopup:ServerCacheInMinutes");

        public static int UMAnnouncementPopupClientIntervalInMinutes => GetValue<int>("UMAnnouncementPopup:ClientIntervalInMinutes");

        public static string[] DomainsWithViva88ContactInfo => GetSectionValue<string[]>("newdomains");

        public static bool IsViva88Domain
            => DomainsWithViva88ContactInfo.Any(domain => domain.Equals(AlphaContext.Current.Request.Host.Value, StringComparison.OrdinalIgnoreCase));

        public static bool IsOfferLiveChat => GetValue<bool>("IsOfferLiveChat");

        public static string ContactUsSkypeId => GetValue<string>("skypeId");

        public static string ContactUsLetstalkId => GetValue<string>("letstalkid");

        public static string ContactUsLetstalkIdNew => GetValue<string>("letstalkidNew");

        public static string ContactUsLetstalkUrl => GetValue<string>("letstalkurl");

        public static string ContactUsLetstalkUrlNew => GetValue<string>("letstalkurlNew");

        public static string[] ContactUsNewDomains => GetSectionValue<string[]>("newdomains");

        public static int CDNReCheckinIntervalInSeconds => GetValue("DNReCheckin:IntervalInSeconds", 60);

        public static int RecheckinIntervalMiliseconds => GetValue("Recheckin:IntervalMiliseconds", 30000);

        public static string JsVersion => GetValue<string>("JsVersion");

        public static string APIGatewaysURL => GetValue<string>("ApiKey:Gateway");

        public static string[] APISuperIds => GetSectionValue<string[]>("ApiKey:AllowSuperIds");

        public static bool HideQRCode => GetValue<bool>("QRCode:Hide");

        private static T GetValue<T>(string key, T defaultValue = default) => ConfigurationManager.GetValue<T>(key, defaultValue);

        private static T GetSectionValue<T>(string key, T defaultValue = default) => ConfigurationManager.GetSection<T>(key, defaultValue);
    }
}