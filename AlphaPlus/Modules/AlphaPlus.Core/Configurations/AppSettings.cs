namespace AlphaPlus.Core.Configurations;

public static class AppSettings
{
    private static DateTime? forceLoginNicknameDate;
    private static IDictionary<int, bool> forceLoginNicknameIgnoreList;
    private static DateTime? forceLoginNicknameDate4SubAccount;

    static AppSettings()
    {
        GetForceLoginNicknameConfiguration();
    }

    public static bool EnableKickOutWhenSessionInvalid => ConfigurationManager.GetValue("EnableKickOutWhenSessionInvalid", false);

    public static string SessionVerificationTokenCookie => ConfigurationManager.GetValue("SessionVerificationTokenCookie", "__SessionVerificationToken_");

    public static bool EnableSessionValidation => ConfigurationManager.GetValue("EnableSessionValidation", false);

    public static bool EnableAntiXSRF => ConfigurationManager.GetValue("EnableAntiXSRF", true);

    public static string AssetAppPath => ConfigurationManager.GetValue("AssetAppPath", "assets");

    public static string ReportsAppPath => ConfigurationManager.GetValue("ReportsAppPath", "/reports");

    public static string BetListsAppPath => ConfigurationManager.GetValue("BetListsAppPath", "/tickets");

    public static string MemberInfoAppPath => ConfigurationManager.GetValue("MemberInfoAppPath", "/settings");

    public static string MessageAppPath => ConfigurationManager.GetValue("MessageAppPath", "/features");

    public static string AccountInfoAppPath => ConfigurationManager.GetValue("AccountInfoAppPath", "/account-info");

    public static string StaticResourcesVersion => ConfigurationManager.GetValue("AssetsApp:StaticResourcesVersion", "100");

    public static string GoogleAnalyticUrl => ConfigurationManager.GetValue("GoogleAnalyticUrl", "https://www.google-analytics.com/analytics.js");

    public static string GoogleAnalyticId => ConfigurationManager.GetValue<string>("GoogleAnalyticId");

    public static string ExTotalBetsForecastAppPath => ConfigurationManager.GetValue("ExTotalBetsForecastAppPath", "ex-totalbetsforecast");

    public static string HowToUseAppPath => ConfigurationManager.GetValue("HowToUseAppPath", "/features");

    public static string TransferAppPath => ConfigurationManager.GetValue("TransferAppPath", "/settings");

    public static string ViewLogsAppPath => ConfigurationManager.GetValue("ViewLogsAppPath", "/settings");

    public static DateTime? ForceLoginNicknameDate => forceLoginNicknameDate;

    public static IDictionary<int, bool> ForceLoginNicknameIgnoreList => forceLoginNicknameIgnoreList;

    public static DateTime? ForceLoginNicknameDate4SubAccount => forceLoginNicknameDate4SubAccount;

    public static bool UpdateCreditBothDBCustAndDBAge => ConfigurationManager.GetValue<string>("UpdateCreditReplicateServer") == "1";

    public static int InsertProfilePictureSiteMode => ConfigurationManager.GetValue<int>("InsertProfilePictureSiteMode");

    public static string ProductConfigsFileRelativePath => ConfigurationManager.GetValue<string>("ProductConfigsFileRelativePath");

    public static string QRCodeMemberSiteLoginConfigsPath => ConfigurationManager.GetValue<string>("QRCodeMemberSiteLoginConfigs");

    public static string[] HideProductStatusSites => ConfigurationManager.GetValue<string>("HideProductStatus:Sites")?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? [];

    public static string SiteLabel => ConfigurationManager.GetValue<string>("SiteLabel");

    public static bool IsSiteLabel789y => SiteLabel.Equals("789y");

    public static bool IsSiteLabel777 => SiteLabel.Equals("777");

    public static bool DisabledVietnamese => !IsSiteLabel789y;

    public static string SiteLabel789y => ConfigurationManager.GetValue<string>("SiteLabel789y");

    public static byte SettingStatus => ConfigurationManager.GetValue<byte>("SettingStatus");

    public static string DBTXN_IP => ConfigurationManager.GetValue<string>("DBTXN_IP");

    public static string SubAccountPermission_LockAdminAccounts => ConfigurationManager.GetValue<string>("SubAccountPermission_LockAdminAccounts");

    public static string ProductColumnNameMappings => ConfigurationManager.GetValue<string>("ProductColumnNameMappings");

    public static string AllowedListSuperAccounts => ConfigurationManager.GetValue<string>("AllowedListSuperAccounts");

    public static bool DisabledCheckDuplicationLogin => ConfigurationManager.GetValue<string>("DisabledCheckDuplicationLogin") == "1";

    public static string JsVersion => ConfigurationManager.GetValue<string>("JsVersion");

    public static bool LocalTest => ConfigurationManager.GetValue<bool>("LocalTest");

    public static int PageSize => ConfigurationManager.GetValue<int>("PageSize");

    public static bool IsEnabledProduct(int product) => ConfigurationManager.GetValue<string>("EnableProduct-" + product) == "1";

    public static HashSet<int> ForcedHideProductIds { get; } = new HashSet<int>(
        ConfigurationManager
            .GetValue<string>("ForcedHideProductIds")
            .Split(',')
            .Select(v => int.Parse(v)));

    public static bool SupportedNewHandbook
    {
        get
        {
            var marketsSupportedNewHandbook = ConfigurationManager.GetValue<string>("MarketsSupportedNewHandbook");
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
