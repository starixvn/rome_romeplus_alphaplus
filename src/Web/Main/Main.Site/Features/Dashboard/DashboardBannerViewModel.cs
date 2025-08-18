using AlphaPlus.Core.Models;
using AlphaPlus.Main.Site.Features.Dashboard.Banner;

namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardBannerViewModel(IUserSession userSession, IConfiguration configuration)
    {
        private readonly IUserSession userSession = userSession;
        private readonly IConfiguration configuration = configuration;

        public bool IsShow { get; private set; } = AppSettings.DashboardBannerOfferedSites.Contains("*") || AppSettings.DashboardBannerOfferedSites.Contains(userSession.Site);

        public IEnumerable<BannerConfiguration> Banners => IsShow ? GetBannerConfiguration() : [];

        private IEnumerable<BannerConfiguration> GetBannerConfiguration()
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var banners = configuration.GetSection($"Banners:{folder}").GetChildren();
            return banners.Select(selector => new BannerConfiguration
            {
                Name = selector.GetValue<string>("Name"),
                ImageSource = selector["ImageSource_" + userSession.Language] ?? selector["ImageSource_" + Shared.Constants.Language.Default],
                ImageMobileSource = selector["ImageMobileSource_" + userSession.Language] ?? selector["ImageMobileSource_" + Shared.Constants.Language.Default],
                Url = selector.GetValue<string>("Url"),
                IsVideo = selector.GetValue<bool>("IsVideo"),
                LogClick = selector.GetValue<bool>("LogClick")
            });
        }
    }
}