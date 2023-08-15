using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaPlus.Configurations
{
    public static class AppSettings
    {
        private static DateTime? forceLoginNicknameDate;
        private static IDictionary<int, bool> forceLoginNicknameIgnoreList;
        private static DateTime? forceLoginNicknameDate4SubAccount;

        static AppSettings()
        {
            GetForceLoginNicknameConfiguration();
        }

        public static string OldMainAppPath => ConfigurationManager.GetValue("OldMainAppPath", "ex-main");

        public static bool EnableKickOutWhenSessionInvalid => ConfigurationManager.GetValue("EnableKickOutWhenSessionInvalid", false);

        public static string SessionVerificationTokenCookie => ConfigurationManager.GetValue("SessionVerificationTokenCookie", "__SessionVerificationToken_");

        public static bool EnableSessionValidation => ConfigurationManager.GetValue("EnableSessionValidation", false);

        public static bool EnableAntiXSRF => ConfigurationManager.GetValue("EnableAntiXSRF", true);

        public static string MainAppPath => ConfigurationManager.GetValue("MainAppPath", "site-main");

        public static string AssetAppPath => ConfigurationManager.GetValue("AssetAppPath", "assets");

        public static string ReportsAppPath => ConfigurationManager.GetValue("ReportsAppPath", "site-reports");

        public static string BetListsAppPath => ConfigurationManager.GetValue("BetListsAppPath", "site-betlists");

        public static string MemberInfoAppPath => ConfigurationManager.GetValue("MemberInfoAppPath", "site-memberinfo");

        public static string MessageAppPath => ConfigurationManager.GetValue("MessageAppPath", "site-message");

        public static string AccountInfoAppPath => ConfigurationManager.GetValue("AccountInfoAppPath", "site-accountinfo");

        public static string StaticResourcesVersion => ConfigurationManager.GetValue("AssetsApp:StaticResourcesVersion", "100");

        public static string GoogleAnalyticUrl => ConfigurationManager.GetValue("GoogleAnalyticUrl", "https://www.google-analytics.com/analytics.js");

        public static string GoogleAnalyticId => ConfigurationManager.GetValue<string>("GoogleAnalyticId");

        public static string ExTotalBetsForecastAppPath => ConfigurationManager.GetValue("ExTotalBetsForecastAppPath", "ex-totalbetsforecast");

        public static string HowToUseAppPath => ConfigurationManager.GetValue("HowToUseAppPath", "site-howtouse");

        public static string TransferAppPath => ConfigurationManager.GetValue("TransferAppPath", "site-transfer");

        public static string ViewLogsAppPath => ConfigurationManager.GetValue("ViewLogsAppPath", "site-viewLogs");

        public static DateTime? ForceLoginNicknameDate => forceLoginNicknameDate;

        public static IDictionary<int, bool> ForceLoginNicknameIgnoreList => forceLoginNicknameIgnoreList;

        public static DateTime? ForceLoginNicknameDate4SubAccount => forceLoginNicknameDate4SubAccount;

        public static bool UpdateCreditBothDBCustAndDBAge => ConfigurationManager.GetValue<string>("UpdateCreditReplicateServer") == "1";

        public static int InsertProfilePictureSiteMode => ConfigurationManager.GetValue<int>("InsertProfilePictureSiteMode");

        public static string ProductConfigsFileRelativePath => ConfigurationManager.GetValue<string>("ProductConfigsFileRelativePath");

        public static string QRCodeMemberSiteLoginConfigsPath => ConfigurationManager.GetValue<string>("QRCodeMemberSiteLoginConfigs");

        public static string[] HideProductStatusSites => ConfigurationManager.GetValue<string>("HideProductStatus:Sites")?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };

        public static string SiteLabel => ConfigurationManager.GetValue<string>("SiteLabel");

        public static bool IsSiteLabel789y => SiteLabel.Equals("789y");

        public static bool ShowOldCustomerList => ConfigurationManager.GetValue<string>("ShowOldCustomerList") == "1";

        private static void GetForceLoginNicknameConfiguration()
        {
            var forceLoginNicknameDateValue = ConfigurationManager.GetValue<string>("ForceLoginNicknameDate");
            forceLoginNicknameDate = string.IsNullOrWhiteSpace(forceLoginNicknameDateValue)
                ? default(DateTime?)
                : DateTime.ParseExact(forceLoginNicknameDateValue, "dd/MM/yyyy", null);

            var forceLoginNicknameIgnoreListValue = ConfigurationManager.GetValue<string>("ForceLoginNicknameIgnoreList");
            forceLoginNicknameIgnoreList = string.IsNullOrWhiteSpace(forceLoginNicknameIgnoreListValue)
                ? new Dictionary<int, bool>()
                : forceLoginNicknameIgnoreListValue
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToDictionary(kvp => int.Parse(kvp), kvp => true);

            var forceLoginNicknameDate4SubAccountValue = ConfigurationManager.GetValue<string>("ForceLoginNicknameDate4SubAccount");
            forceLoginNicknameDate4SubAccount = string.IsNullOrWhiteSpace(forceLoginNicknameDate4SubAccountValue)
                ? default(DateTime?)
                : DateTime.ParseExact(forceLoginNicknameDate4SubAccountValue, "dd/MM/yyyy", null);
        }
    }
}