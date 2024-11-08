using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Features.Messages.API.Configurations
{
    public static class AppSettings
    {
        public static bool LocalTest => ConfigurationManager.GetValue<bool>("LocalTest");

        public static string SiteLabel => GetConfigValue("SiteLabel");

        public static string Environment => GetConfigValue("Environment");

        public static string StoreProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string FanexDataServiceApi => GetConfigValue("FanexDataServiceApi");

        public static string MessageSuper => string.IsNullOrWhiteSpace(GetConfigValue("Message:Super")) ? "3000,3100" : GetConfigValue("Message:Super");

        public static string MessageMaster => string.IsNullOrWhiteSpace(GetConfigValue("Message:Master")) ? "3000,3200" : GetConfigValue("Message:Master");

        public static string MessageAgent => string.IsNullOrWhiteSpace(GetConfigValue("Message:Agent")) ? "3000,3300" : GetConfigValue("Message:Agent");

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string GetConfigValue(string configName) => ConfigurationManager.GetValue<string>(configName);

        public static bool EnableLogInfo => GetConfigValue("EnableLogInfo") == "1";

        public static bool IsDisabledRecheckin => GetConfigValue("DisabledCheckDuplicationLogin") == "1";

        public static bool DisableMessages => "1" == GetConfigValue("DisableMessages");

        public static string JsVersion => GetConfigValue("JsVersion");

        public static string AnnouncementApi => GetConfigValue("Announcement:ApiUrl");

        public static string AnnouncementSecretKey
            => string.IsNullOrEmpty(GetConfigValue("Announcement:SecretKey")) ?
            "ZDlhMWFmMDgtZGQ0ZC00NWY0LTg1ODItNzVkNTdhOTNjOTEx" :
            GetConfigValue("Announcement:SecretKey");

        public static void Verify()
        {
            Require(StoreProcedurePath, nameof(StoreProcedurePath));
            Require(Environment, nameof(Environment));
        }

        private static void Require(string key, string keyName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new NullReferenceException($"{keyName} is null or whitespace");
            }
        }
    }
}