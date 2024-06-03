using System.Text.Json;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class CustomerBaseViewModel
    {
        public CustomerBaseViewModel(AppConfiguration appConfig, string cndUri, string assetsVersion)
        {
            AppConfig = appConfig != null ? JsonSerializer.Serialize(appConfig) : string.Empty;
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string Params { get; protected set; }
    }
}