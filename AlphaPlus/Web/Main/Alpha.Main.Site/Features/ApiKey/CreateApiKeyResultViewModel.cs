namespace Alpha.Main.Site.Features.ApiKey
{
    public class CreateApiKeyResultViewModel
    {
        public CreateApiKeyResultViewModel(string apiName, string key, string secret)
        {
            ApiName = apiName;
            Key = key;
            Secret = secret;
        }

        public string ApiName { get; }

        public string Key { get; }

        public string Secret { get; }
    }
}