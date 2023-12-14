
using AlphaPlus.Core.Configurations;

namespace AlphaPlus.PartialUM.Configurations
{
    internal static class AppSettings
    {
        static AppSettings()
        {
            PartialUMReportsBetListsCacheInSeconds = ToInt(ConfigurationManager.GetValue<string>("PartialUM:ReportsBetListsCacheInSeconds"), 15);
        }

        public static int PartialUMReportsBetListsCacheInSeconds { get; }

        private static int ToInt(string value, int defaultValue = 0)
            => string.IsNullOrWhiteSpace(value) || !int.TryParse(value, out var valueInt)
            ? defaultValue
            : valueInt;
    }
}
