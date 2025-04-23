using AlphaPlus.Core.Models;

namespace AlphaPlus.Main.Site.Features.Language
{
    public interface ILanguageService
    {
        Task<Result> UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip);
    }

    public class LanguageService(HttpClient httpClient) : ILanguageService
    {
        private readonly HttpClient httpClient = httpClient;
        public Task<Result> UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip)
            => httpClient.PostAsJsonAsync<Result>("customer/usage-language", new UpdateUsageLanguageCommand(custId, subAccId, languageKey, ip));
    }
}