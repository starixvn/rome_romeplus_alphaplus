using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardBannerViewModel
    {
        private readonly IUserSession userSession;
        private readonly IConfiguration configuration;

        public DashboardBannerViewModel(IUserSession userSession, IConfiguration configuration)
        {
            this.userSession = userSession;
            this.configuration = configuration;
            IsShow = AppSettings.DashboardBannerOfferedSites.Contains("*")
                || AppSettings.DashboardBannerOfferedSites.Contains(userSession.Site);
            if (IsShow)
            {
                SetBannerUrls(userSession);
            }
        }

        public bool IsShow { get; private set; }

        public string BannerName { get; private set; }

        public string BannerMobileName { get; private set; }

        public IEnumerable<Banner> Banners => GetBannerConfiguration();

        private void SetBannerUrls(IUserSession userSession)
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var language = Shared.Constants.Language.MapToMainLanguageCode(userSession.Language);
            var version = AppSettings.JsVersion;
            BannerName = $"{folder}/banner_{language}.jpg?v={version}";
            BannerMobileName = $"{folder}/banner_mobile_{language}.jpg?v={version}";
        }

        private IEnumerable<Banner> GetBannerConfiguration()
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var banners = configuration.GetSection($"Banners:{folder}").GetChildren();
            return banners.Select(b => new Banner
            {
                ImageSource = b["ImageSource_" + userSession.Language] ?? b["ImageSource_" + Shared.Constants.Language.Default],
                ImageMobileSource = b["ImageMobileSource_" + userSession.Language] ?? b["ImageMobileSource_" + Shared.Constants.Language.Default],
                Url = b.GetValue<string>("Url"),
                IsVideo = b.GetValue<bool>("IsVideo")
            });
        }
    }
}