using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Settings.Transfer.API.Configurations;

public static class AppSettings
{
    public static string GetConfigValue(string configName) => ConfigurationManager.GetValue<string>(configName);


    public static IEnumerable<string> TransferToDownlineHistoryOfferSites
        => GetConfigValue("TransferToDownlineHistory:OfferSites")
        .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(site => site.Trim());

    public static bool IsAllowedTransferToDownlineHistorySite(string site)
        => TransferToDownlineHistoryOfferSites.Contains("*", StringComparer.CurrentCultureIgnoreCase)
        || TransferToDownlineHistoryOfferSites.Contains(site, StringComparer.CurrentCultureIgnoreCase);

    public static string SiteLabel => GetConfigValue("SiteLabel") ?? string.Empty;

    public static int MultipleTransferCacheTimeOut => int.Parse(GetConfigValue("MultipleTransferCacheTimeOut"));

    public static bool EnableTransferLogInfo => GetConfigValue("EnableTransferLogInfo") == "1";

    public static string TransferLogCategory => GetConfigValue("TransferLogCategory");

    public static bool CheckOfferAutoTransferCurrencies(string currency)
        => OfferAutoTransferCurrencies.Contains("*") || OfferAutoTransferCurrencies.Contains(currency);

    private static HashSet<string> OfferAutoTransferCurrencies { get; } = new HashSet<string>(
        (GetConfigValue("OfferAutoTransfer.Currencies") ?? string.Empty)
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.Trim()),
        StringComparer.OrdinalIgnoreCase
    );

    public static int MaxCustIdTransfer => int.Parse(GetConfigValue("MaxCustIdTransfer"));

    public static string VirtualDirectory
    {
        get
        {
            return AppPathHelper.GetAppPath("/settings", "~/");
        }
    }
}