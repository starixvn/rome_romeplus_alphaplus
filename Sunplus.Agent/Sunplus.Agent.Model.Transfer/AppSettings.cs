
using System.Configuration;

namespace Sunplus.Agent.Model.Transfer
{
    public static class AppSettings
    {
        public static string AllowedListSuperAccounts => GetValue("AllowedListSuperAccounts");

        public static int PageSize => int.Parse(GetValue("PageSize"));

        public static string SiteLabel => GetValue("SiteLabel");

        public static int MaxCustIdTransfer => int.Parse(GetValue("MaxCustidTransfer"));

        public static int MultipleTransferCacheTimeOut => int.Parse(GetValue("MultipleTransferCacheTimeOut"));

        public static bool EnableTransferLogInfo => GetValue("EnableTransferLogInfo") == "1";

        public static string TransferLogCategory => GetValue("TransferLogCategory");

        public static string GetValue(string key) => ConfigurationManager.AppSettings[key];
    }
}