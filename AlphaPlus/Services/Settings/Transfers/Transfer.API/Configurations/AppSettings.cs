namespace AlphaPlus.Settings.Transfer.API.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Configurations;
    using AlphaPlus.Core.Helpers;

    public static class AppSettings
    {
        public static string StoreProcedurePath => GetConfigValue("StoredProcedurePath");

        public static string FanexDataServiceApi => GetConfigValue("FanexDataServiceApi");

        public static string AllowedTotalBalanceTransferSuperIds => GetConfigValue("AllowedTotalBalanceTransferSuperIds");

        public static string TransferRedisConfigurationOptions => GetConfigValue("RedisConfigurationOptions:AlphaPlus.Settings.Transfer.API");

        public static string GetConfigValue(string configName) => ConfigurationManager.GetValue<string>(configName);

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

        public static IEnumerable<string> TransferToDownlineHistoryOfferSites
            => GetConfigValue("TransferToDownlineHistory:OfferSites")
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(site => site.Trim());

        public static bool IsAllowedTransferToDownlineHistorySite(string site)
            => TransferToDownlineHistoryOfferSites.Contains("*", StringComparer.CurrentCultureIgnoreCase)
            || TransferToDownlineHistoryOfferSites.Contains(site, StringComparer.CurrentCultureIgnoreCase);

        public static string VirtualDirectory
        {
            get
            {
                return AppPathHelper.GetAppPath(GetConfigValue("VirtualDirectory"), "~/");
            }
        }

        public static string SiteLabel => GetConfigValue("SiteLabel") ?? string.Empty;

        public static bool IsMBCSite => !SiteLabel.Equals("789y", StringComparison.OrdinalIgnoreCase);

        public static int MultipleTransferCacheTimeOut => int.Parse(GetConfigValue("MultipleTransferCacheTimeOut"));

        public static bool EnableTransferLogInfo => GetConfigValue("EnableTransferLogInfo") == "1";

        public static string TransferLogCategory => GetConfigValue("TransferLogCategory");
    }
}