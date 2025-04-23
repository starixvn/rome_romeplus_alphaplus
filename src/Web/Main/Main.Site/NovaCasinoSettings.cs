namespace AlphaPlus.Main.Site
{
    using AlphaPlus.Core.Configurations;

    public static class NovaCasinoSettings
    {
        public static string ServiceUrl => GetValue<string>("NovaCasino:ServiceUrl");

        public static string SecretKey => GetValue<string>("NovaCasino:SecretKey");

        public static bool IsOfferedNovaCasino(string site, string level, string currency)
           => (NovaCasinoOfferedSites.Contains("*") || NovaCasinoOfferedSites.Contains(site))
           && (NovaCasinoOfferedCurrencies.Contains("*") || NovaCasinoOfferedCurrencies.Contains(currency))
           && DateTime.Parse(GetValue<string>($"NovaCasino:OfferedDate:{level}")) <= DateTime.Today;


        private static IEnumerable<string> NovaCasinoOfferedSites { get; } = new HashSet<string>(
            (GetValue<string>("NovaCasino:OfferedSites") ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(site => site.Trim()), StringComparer.OrdinalIgnoreCase);

        private static IEnumerable<string> NovaCasinoOfferedCurrencies { get; } = new HashSet<string>(
           (GetValue<string>("NovaCasino:OfferedCurrencies") ?? string.Empty)
               .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
               .Select(site => site.Trim()), StringComparer.OrdinalIgnoreCase);

        private static T GetValue<T>(string key, T defaultValue = default) => ConfigurationManager.GetValue<T>(key, defaultValue);
    }
}