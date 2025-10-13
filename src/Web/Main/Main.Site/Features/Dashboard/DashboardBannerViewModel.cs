using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Dashboard.Banner;

namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardBannerViewModel(IUserSession userSession, IConfiguration configuration)
    {
        private readonly IUserSession userSession = userSession;
        private readonly IConfiguration configuration = configuration;

        private readonly string CASH_SERVICE_BANNERS_SECTION = "Banners:CashService";

        public bool IsShow { get; private set; } = AppSettings.DashboardBannerOfferedSites.Contains("*") || AppSettings.DashboardBannerOfferedSites.Contains(userSession.Site);

        public IEnumerable<BannerConfiguration> Banners => IsShow ? GetBannerConfiguration() : [];

        private IEnumerable<BannerConfiguration> GetBannerConfiguration()
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var folderPath = $"Banners:{folder}";
            
            var bannerConfigurations = GetBannerConfigurationsFromFolder(folderPath);
            return GetBannerConfigurationWithCashService(bannerConfigurations);
        }

        private IEnumerable<BannerConfiguration> GetBannerConfigurationWithCashService(IEnumerable<BannerConfiguration> bannerConfigurations)
        {
            var isOfferCash = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(userSession.Site);
            
            if (!isOfferCash)
            {
                return bannerConfigurations;    
            }

            var cashServiceBanners = GetBannerConfigurationsFromFolder(CASH_SERVICE_BANNERS_SECTION);
            return cashServiceBanners.Concat(bannerConfigurations);
        }

        private IEnumerable<BannerConfiguration> GetBannerConfigurationsFromFolder(string folderPath)
        {
            var banners = configuration.GetSection(folderPath).GetChildren();
            
            return banners.Select(selector => new BannerConfiguration
            {
                Name = selector.GetValue<string>("Name") ?? string.Empty,
                ImageSource = GetLocalizedValue(selector, "ImageSource"),
                ImageMobileSource = GetLocalizedValue(selector, "ImageMobileSource"),
                Url = selector.GetValue<string>("Url") ?? string.Empty,
                IsVideo = selector.GetValue<bool>("IsVideo"),
                LogClick = selector.GetValue<bool>("LogClick")
            });
        }

        private string GetLocalizedValue(IConfigurationSection selector, string key)
        {
            var localizedKey = $"{key}_{userSession.Language}";
            var defaultKey = $"{key}_{Shared.Constants.Language.Default}";
            
            return selector[localizedKey] ?? selector[defaultKey] ?? string.Empty;
        }
    }
}