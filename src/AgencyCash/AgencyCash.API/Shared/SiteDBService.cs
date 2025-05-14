using AgencyCash.API.Shared.Criteria;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AgencyCash.API.Shared;

public interface ISiteDBService
{
    Task<int> GetCustDBGroupId(int custId);

    Task<SiteDBConfig> GetSiteDBConfig(int custId);
}

public class SiteDBService(IDynamicRepository dynamicRepository, IMemoryCache memoryCache) : ISiteDBService
{
    private const int SiteDBConfigCacheInMinutes = 5;
    private const string SiteDBConfigCacheKey = "AgencyCash.API.CustDBGroupId";

    private readonly IDynamicRepository dynamicRepository = dynamicRepository;
    private readonly IMemoryCache memoryCache = memoryCache;

    public async Task<int> GetCustDBGroupId(int custId)
    {
        var siteDBConfig = await GetSiteDBConfig(custId);

        return siteDBConfig?.CustDBGroupId ?? 0;
    }

    public async Task<SiteDBConfig> GetSiteDBConfig(int custId)
    {
        var key = $"{SiteDBConfigCacheKey}_{custId}";

        if (!memoryCache.TryGetValue<SiteDBConfig>(key, out var siteDBConfig))
        {
            siteDBConfig = await dynamicRepository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(custId));
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(SiteDBConfigCacheInMinutes));
            memoryCache.Set(key, siteDBConfig, cacheEntryOptions);
        }

        return siteDBConfig;
    }
}