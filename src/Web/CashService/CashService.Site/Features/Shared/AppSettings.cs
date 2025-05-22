
namespace AlphaPlus.CashService.Site.Features.Shared
{
    using AlphaPlus.Core.Configurations;

    public class AppSettings
    {
        public static bool IsOfferedCashService(string site)
            => CashServiceOfferedSites.Contains("*") || CashServiceOfferedSites.Contains(site);

        public static IEnumerable<string> CashServiceOfferedSites { get; } = new HashSet<string>(
            (GetValue<string>("CashService:OfferedSites") ?? string.Empty)
               .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(site => site.Trim()), StringComparer.OrdinalIgnoreCase);

        private static T GetValue<T>(string key, T defaultValue = default) => ConfigurationManager.GetValue<T>(key, defaultValue);

        private static T GetSectionValue<T>(string key, T defaultValue = default) => ConfigurationManager.GetSection<T>(key, defaultValue);
    }
}