using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using Microsoft.AspNetCore.Http;

namespace AlphaPlus.Settings.MemberInfo.Domain.Configurations
{
    public static class AppSettings
    {
        [ThreadStatic]
        private static HttpContext context = AlphaContext.Current;

        public static string StoreProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string FanexDataServiceApi => GetConfigValue("FanexDataServiceApi");

        public static string SiteLabel => GetConfigValue("SiteLabel");

        public static string SiteLabel789y => GetConfigValue("SiteLabel789y");

        public static bool IsSiteLabel789y => SiteLabel.Equals(SiteLabel789y);

        public static string[] SupportPT3DecimalSites => GetConfigValue("SupportPT3DecimalSites")?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? [];

        public static bool IsSupportPT3DecimalSites => SupportPT3DecimalSites.Contains(SiteLabel, StringComparer.InvariantCultureIgnoreCase);

        public static string GetConfigValue(string configName) => Core.Configurations.ConfigurationManager.GetValue<string>(configName);

        public static void Verify()
        {
            Require(StoreProcedurePath, nameof(StoreProcedurePath));
        }

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NullReferenceException($"{keyName} is null or whitespace");
            }
        }

        public static string OldMainAppPath => GetConfigValue("OldMainAppPath");

        public static string MainAppPath => GetConfigValue("MainAppPath");

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string TransferAppPath => GetConfigValue("TransferAppPath");

        public static string IndirectLoginURL
            => !string.IsNullOrWhiteSpace(GetConfigValue("IndirectLogin:URL")) ?
                $"{GetConfigValue("IndirectLogin:URL")}" :
                $"//{context.Request.Headers["Host"].ToString().Replace("mb.", "mb2.")}/ex-main/_Authorization/IndirectLogin.aspx";

        public static IEnumerable<int> IndirectLoginSuperIds
            => GetConfigValue("IndirectLogin:AllowSuperIds")
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select
            (
                id => string.IsNullOrWhiteSpace(id) ? 0 : ValueOfNotNullOrWhiteSpace(id)
            );

        public static bool IsAllowedIndirectLogin(int srecommend)
            => IndirectLoginSuperIds.Contains(srecommend) || IndirectLoginSuperIds.Any(id => id == int.MaxValue);

        public static byte SettingStatus
        {
            get
            {
                byte.TryParse(GetConfigValue("SettingStatus"), out byte settingStatus);
                return settingStatus;
            }
        }

        public static byte CreationStatus
        {
            get
            {
                byte.TryParse(GetConfigValue("CreationStatus"), out byte creationStatus);
                return creationStatus;
            }
        }

        private static int ValueOfNotNullOrWhiteSpace(string value)
            => value.Equals("*", StringComparison.OrdinalIgnoreCase) ? int.MaxValue : Convert.ToInt32(value);

        public static IEnumerable<string> NewCaptchaLogCurrencies => string.IsNullOrWhiteSpace(GetConfigValue("NewCaptcha:LogCurrencies"))
            ? Enumerable.Empty<string>()
            : GetConfigValue("NewCaptcha:LogCurrencies").Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

        public static string JsVersion => GetConfigValue("JsVersion");

        public static bool LocalTest => ConfigurationManager.GetValue<bool>("LocalTest");

        public static bool IsMBCSite => !IsSiteLabel789y;

        public static int MaxAccountsCreatePerRequest => Convert.ToInt32(GetConfigValue("CreateNewAccount:MaxAccountPerRequest"));

        public static Uri CustomerApiUri => new Uri(GetConfigValue("CustomerApi:Uri"));

        public static bool IsStandaloneBetlist => !string.IsNullOrWhiteSpace(GetConfigValue("Betlist:Domain"));

        public static int SystemId => Convert.ToInt32(GetConfigValue("SystemId"));

        public static string BetlistDomain
        {
            get
            {
                var betlistDomain = string.Empty + GetConfigValue("Betlist:Domain");
                var mainDomain = context.Request.Headers["Host"].ToString();

                if (UserSession.Current.IsInternal)
                {
                    mainDomain = mainDomain.Replace($"{mainDomain.Split('.')?.ElementAt(0)}.", string.Empty);
                }

                return betlistDomain.IndexOf("{0}", StringComparison.OrdinalIgnoreCase) != -1
                    ? string.Format(betlistDomain, mainDomain)
                    : betlistDomain;
            }
        }

        public static bool IsOfferCreateAccountLiteMode => GetConfigValue("CreateNewAccount:OfferLiteMode") == "1";
    }
}