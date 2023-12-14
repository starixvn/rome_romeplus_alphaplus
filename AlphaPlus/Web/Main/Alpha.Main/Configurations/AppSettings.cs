using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;

namespace Alpha.Main.Configurations
{
    public static class AppSettings
    {
        public static string VirtualDirectory => GetValue<string>("VirtualDirectory");

        public static string NonUseSecurityCodeSites => GetValue<string>("NonUseSecurityCodeSites");

        public static string StaticResourcesVersion => GetValue<string>("AssetsApp:StaticResourcesVersion");

        public static string[] DevicesUseCustomScrollbar => GetValue<string[]>("DevicesUseCustomScrollbar");

        public static string StoreProcedurePath => GetValue<string>("StoredProcedurePath");

        public static string MessageSuper => GetValue<string>("Message:Super");

        public static string MessageMaster => GetValue<string>("Message:Master");

        public static string MessageAgent => GetValue<string>("Message:Agent");

        public static bool IsDisabledContactUsLink => GetValue<bool>("IsDisabledContactUsLink");

        public static string GoogleAnalyticId => GetValue<string>("GoogleAnalyticId");

        public static bool DisableMessages => GetValue<bool>("DisableMessages");

        public static bool IsMBCSite => !IsSiteLabel789y;

        public static bool IsSiteLabel789y => SiteLabel.Equals(SiteLabel789y);

        public static bool DisabledVietnamese => IsMBCSite;

        public static bool ShowHelpLink => IsMBCSite;

        public static string[] NoticeTypes => GetValue<string[]>("NoticeTypes");

        public static string GetAppPath(string name) => GetValue<string>(name);

        public static bool LocalTest => GetValue<bool>("LocalTest");

        public static int[] TrackingConnectionCustIds => GetValue<int[]>("TrackingConnectionCustIds");

        public static string RemoteRefURL => GetValue<string>("RemoteRefURL");

        public static string CreatedSettingLastDateSessionKey => "CreatedSettingLastDate";

        public static bool BannerIsHide => GetValue<bool>("Banner:IsHide");

        public static string BannerEnvironment => GetValue<string>("Banner:Environment");

        public static bool BannerIsHideMobileSabaApp => GetValue<bool>("Banner:IsHideMobileSabaApp");

        public static string SiteLabel => GetValue<string>("SiteLabel");

        public static string SiteLabel789y => GetValue<string>("SiteLabel789y");

        public static bool EnableUMAnnouncementPopup => GetValue<bool>("EnableUMAnnouncementPopup");

        public static int UMAnnouncementPopupServerCacheInMinutes => GetValue<int>("UMAnnouncementPopup:ServerCacheInMinutes");

        public static int UMAnnouncementPopupClientIntervalInMinutes => GetValue<int>("UMAnnouncementPopup:ClientIntervalInMinutes");

        public static string[] NewCaptchaLogCurrencies => GetValue<string[]>("NewCaptcha:LogCurrencies");


        public static bool IsShowEmptyDashboard => GetValue<bool>("IsShowEmptyDashboard");

        public static string[] DomainsWithViva88ContactInfo => GetValue<string[]>("newdomains");

        public static bool IsViva88Domain
            => DomainsWithViva88ContactInfo.Any(domain => domain.Equals(AlphaContext.Current.Request.Host.Value, StringComparison.OrdinalIgnoreCase));

        public static bool IsOfferLiveChat => GetValue<bool>("IsOfferLiveChat");

        public static string ContactUsSkypeId => GetValue<string>("skypeId");

        public static string ContactUsLetstalkId => GetValue<string>("letstalkid");

        public static string ContactUsLetstalkIdNew => GetValue<string>("letstalkidNew");

        public static string ContactUsLetstalkUrl => GetValue<string>("letstalkurl");

        public static string ContactUsLetstalkUrlNew => GetValue<string>("letstalkurlNew");

        public static string ContactUsNewDomains => GetValue<string>("newdomains");

        public static int CDNReCheckinIntervalInSeconds => GetValue("DNReCheckin:IntervalInSeconds", 60);

        public static int RecheckinIntervalMiliseconds => GetValue("Recheckin:IntervalMiliseconds", 3000);

        public static string JsVersion => GetValue<string>("JsVersion");
        public static Uri ApiKeyServiceUri => new(GetValue<string>("ApiKeyServiceUri"));
        public static bool IsStoreUserLangEnabled => GetValue<bool>("IsStoreUserLangEnabled");

        public static string APIGatewaysURL => GetValue<string>("APIGatewaysURL");

        public static string[] APISuperIds => GetValue<string[]>("APISuperIds");

        public static bool HideQRCode => GetValue<bool>("QRCode:Hide");

        private static T GetValue<T>(string key, T defaultValue = default) => ConfigurationManager.GetValue<T>(key, defaultValue);
    }
}