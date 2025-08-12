using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Dashboard
{
    public class DashboardBannerViewModel
    {
        public DashboardBannerViewModel(IUserSession userSession, IConfiguration configuration)
        {
            IsShow = AppSettings.DashboardBannerOfferedSites.Contains("*")
                || AppSettings.DashboardBannerOfferedSites.Contains(userSession.Site);
            if (IsShow)
            {
                SetBannerUrls(userSession);
            }

            var banners = configuration.GetSection("Banners").Get<List<Banner>>();
            Banners = [.. banners];
        }

        public bool IsShow { get; private set; }

        public string BannerName { get; private set; }

        public string BannerMobileName { get; private set; }

        public Banner[] Banners { get; private set; }

        private void SetBannerUrls(IUserSession userSession)
        {
            var folder = AppSettings.IsSiteLabel789y ? "i7" : "i1";
            var language = Shared.Constants.Language.MapToMainLanguageCode(userSession.Language);
            var version = AppSettings.JsVersion;
            BannerName = $"{folder}/banner_{language}.jpg?v={version}";
            BannerMobileName = $"{folder}/banner_mobile_{language}.jpg?v={version}";
        }
    }
}