using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Language
{
    public interface ILanguageService
    {
        Task UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip);
    }

    public class LanguageService(IDynamicRepository dynamicRepository) : ILanguageService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        public Task UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip)
            => dynamicRepository.ExecuteAsync(new UpdateUsageLanguageCommand(custId, subAccId, languageKey, ip));
    }
}