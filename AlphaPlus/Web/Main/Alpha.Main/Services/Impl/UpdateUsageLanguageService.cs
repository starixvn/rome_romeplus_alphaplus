namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Criteria.Language;
    using Fanex.Data.Repository;

    public interface UpdateUsageLanguageService
    {
        Task UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip);
    }

    public class UpdateUsageLanguageServiceImpl(IDynamicRepository dynamicRepository) : UpdateUsageLanguageService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public Task UpdateUsageLanguage(int custId, int subAccId, string languageKey, string ip)
            => dynamicRepository.ExecuteAsync(new UpdateUsageLanguageCommand(custId, subAccId, languageKey, ip));
    }
}