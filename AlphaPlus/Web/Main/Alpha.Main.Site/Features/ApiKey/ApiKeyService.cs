using AlphaPlus.Core.Models;
using TelegramBot.Client.Extensions;

namespace Alpha.Main.Site.Features.ApiKey
{
    public interface IApiKeyService
    {
        Task<CreateApiKeyResult> CreateApiKey(int custId, string apiName, string passPhrase, string scopes, string ip, string language, DateTime? expirationDate);

        Task RevokeApiKey(string apiKey, string ip);

        Task<Result> UpdateApiKey(string apiKey, int custId, string apiName, string scopes, string ip, string language, DateTime? expirationDate);

        Task<IEnumerable<ApiKeyInfo>> GetApiKeyByCustId(int custId);

        Task<ApiKeyInfo> GetApiKeyByKey(string key);
    }

    public class ApiKeyService(HttpClient httpClient) : IApiKeyService
    {
        private readonly HttpClient httpClient = httpClient;

        public async Task<CreateApiKeyResult> CreateApiKey(int custId, string apiName, string passPhrase, string scopes, string ip, string language, DateTime? expirationDate)
        {
            var parameter = new { custId, apiName, passPhrase, scopes, ip, language, expirationDate = expirationDate?.AddDays(1).AddSeconds(-1) };
            return await httpClient.PostAsJsonAsync<CreateApiKeyResult>(string.Empty, parameter);
        }

        public async Task<IEnumerable<ApiKeyInfo>> GetApiKeyByCustId(int custId)
        {
            return await httpClient.GetAsJsonAsync<IEnumerable<ApiKeyInfo>>(custId.ToString());
        }

        public async Task<ApiKeyInfo> GetApiKeyByKey(string key)
        {
            return await httpClient.GetAsJsonAsync<ApiKeyInfo>(key);
        }

        public async Task RevokeApiKey(string apiKey, string ip)
        {
            await httpClient.DeleteAsStringAsync($"{apiKey}?ip={ip}");
        }

        public async Task<Result> UpdateApiKey(string apiKey, int custId, string apiName, string scopes, string ip, string language, DateTime? expirationDate)
        {
            var parameter = new { apiKey, custId, apiName, scopes, ip, language, expirationDate = expirationDate?.AddDays(1).AddSeconds(-1) };
            return await httpClient.PutAsJsonAsync<Result>(string.Empty, parameter);
        }
    }
}