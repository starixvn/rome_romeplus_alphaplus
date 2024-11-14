using System.Net;
using System.Net.Http.Headers;
using AlphaPlus.Features.Handbook.API.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class APIHandler
    {
        protected readonly Fanex.Logging.ILogger logger;
        protected string contentAPIUrl = AppSettingsHelper.ContentAPIUrl;

        public APIHandler(Fanex.Logging.ILogger logger) {
            this.logger = logger;
        }

        public T Get<T>(string url)
        {
            try
            {
                var content = Get(url);
                return DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
               logger.Error(e.Message, e);
            }

            return default;
        }

        public string Get(string url)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AppSettingsHelper.ContentAPIToken);
                var response = client.GetAsync(EncodeURL(url)).GetAwaiter().GetResult();
                var content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                return content;
            }
        }

        private T DeserializeObject<T>(string content)
        {
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var result = JsonConvert.DeserializeObject<T>(content, settings);

            return result;
        }

        public static string EncodeURL(string url)
        {
            var result = url.Replace("#", "%23");
            return result;
        }
    }
}