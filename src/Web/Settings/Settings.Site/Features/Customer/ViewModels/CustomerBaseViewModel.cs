using System.Text.Json;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class CustomerBaseViewModel
    {
        private readonly JsonSerializerOptions serializeOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public CustomerBaseViewModel(AppConfiguration appConfig, string cndUri, string assetsVersion)
        {
            AppConfig = appConfig != null ? JsonSerializer.Serialize(appConfig, serializeOptions) : string.Empty;
            CdnUri = cndUri;
            AssetsVersion = assetsVersion;
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string AssetsVersion { get; }

        public string Params { get; protected set; }
    }
}