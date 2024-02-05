using Alpha.Main.Models.ApiKey;

namespace Alpha.Main.Site.Features.ApiKey
{
    public class EditApiKeyViewModel
    {
        public EditApiKeyViewModel(ApiKeyInfo apiKeyInfo, List<string> scopes, string language)
        {
            ApiKeyInfo = apiKeyInfo;
            Scopes = scopes;
            Language = language;
        }

        public ApiKeyInfo ApiKeyInfo { get; }

        public List<string> Scopes { get; }

        public string Language { get; }

        public DateTime CurrentDate => DateTime.Now;
    }
}