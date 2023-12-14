using AlphaPlus.PartialUM.Configurations;
using AlphaPlus.PartialUM.Criteria;
using AlphaPlus.PartialUM.Exceptions;
using AlphaPlus.PartialUM.Helpers;
using AlphaPlus.PartialUM.Models;
using AlphaPlus.Core.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.PartialUM.Services
{
    public interface IPartialUMService
    {
        Task ThrowIfUM(string settingKey);

        Task ThrowIfUM(string settingKey, string siteName);
    }

    public class PartialUMService(ICacheService cacheService, IDynamicRepository dynamicRepository, IPartialUMSectionHelper partialUMSectionHelper)
        : IPartialUMService
    {
        private readonly ICacheService cacheService = cacheService;
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;
        private readonly IPartialUMSectionHelper partialUMSectionHelper = partialUMSectionHelper;

        public Task ThrowIfUM(string settingKey)
            => ThrowIfUM(settingKey, UserSession.Current.Site);

        public async Task ThrowIfUM(string settingKey, string siteName)
        {
            var functionNames = partialUMSectionHelper.GetFunctionNames(settingKey);
            if (!functionNames.Any())
            {
                return;
            }

            var umInfoDictionary = await GetUMInfoDictionary(siteName);
            if (umInfoDictionary.Count == 0)
            {
                return;
            }

            foreach (var function in functionNames)
            {
                if (umInfoDictionary.TryGetValue(function, out var umInfo))
                {
                    throw new PartialUMException(FormatUMTime(umInfo.BeginTime), FormatUMTime(umInfo.EndTime));
                }
            }
        }

        private Task<IDictionary<string, UMInfo>> GetUMInfoDictionary(string siteName)
            => cacheService.GetOrSetAsync(
                $"AlphaPlus.PartialUM.Services.MinimizedUM.SiteName-{siteName}",
                () => GetUMInfoDictionaryFromDB(siteName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(AppSettings.PartialUMReportsBetListsCacheInSeconds)));

        private async Task<IDictionary<string, UMInfo>> GetUMInfoDictionaryFromDB(string siteName)
        {
            var umInfoList = await dynamicRepository.FetchAsync<UMInfo>(new GetUMInfoListCriteria(siteName));
            var umInfoDictionary = new Dictionary<string, UMInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in umInfoList)
            {
                umInfoDictionary[item.FunctionTypeName] = item;
            }

            return umInfoDictionary;
        }

        private static string FormatUMTime(DateTime dateTime)
            => dateTime.ToString("dd/MM/yyyy hh:mm tt");
    }
}