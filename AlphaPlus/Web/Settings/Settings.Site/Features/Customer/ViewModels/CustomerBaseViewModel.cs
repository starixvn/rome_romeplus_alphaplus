using System.Text.Json;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class CustomerBaseViewModel
    {
        public CustomerBaseViewModel(AppConfiguration appConfig, string cndUri)
        {
            AppConfig = appConfig != null ? JsonSerializer.Serialize(appConfig) : string.Empty;
            CdnUri = cndUri;
        }

        public string AppConfig { get; }

        public string CdnUri { get; }

        public string Params { get; protected set; }
    }
}