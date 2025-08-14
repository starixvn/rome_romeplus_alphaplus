using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardBannerViewModel(IUserSession userSession, IConfiguration configuration)
    {
        private readonly IUserSession userSession = userSession;
        private readonly IConfiguration configuration = configuration;

        public bool IsShow { get; private set; } = AppSettings.DashboardBannerOfferedSites.Contains("*") || AppSettings.DashboardBannerOfferedSites.Contains(userSession.Site);

        public IEnumerable<Banner> Banners => IsShow ? GetBannerConfiguration() : [];

        private IEnumerable<Banner> GetBannerConfiguration()
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var banners = configuration.GetSection($"Banners:{folder}").GetChildren();
            return banners.Select(b => new Banner
            {
                Name = b.GetValue<string>("Name"),
                ImageSource = b["ImageSource_" + userSession.Language] ?? b["ImageSource_" + Shared.Constants.Language.Default],
                ImageMobileSource = b["ImageMobileSource_" + userSession.Language] ?? b["ImageMobileSource_" + Shared.Constants.Language.Default],
                Url = b.GetValue<string>("Url"),
                IsVideo = b.GetValue<bool>("IsVideo"),
                LogClick = b.GetValue<bool>("LogClick")
            });
        }
    }
}