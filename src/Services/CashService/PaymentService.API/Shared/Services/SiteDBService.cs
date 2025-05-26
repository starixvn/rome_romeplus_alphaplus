using PaymentService.API.Shared.Criteria;
using PaymentService.API.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace PaymentService.API.Shared;

public interface ISiteDBService
{
    Task<int> GetCustDBGroupId(int custId);

    Task<int> GetSiteDBGroupId(int custId);

    Task<SiteDBConfig> GetSiteDBConfig(int custId);
}

public class SiteDBService(IDynamicRepository dynamicRepository, IMemoryCache memoryCache) : ISiteDBService
{
    private const int SiteDBConfigCacheInMinutes = 5;
    private const string SiteDBConfigCacheKey = "PaymentService.API.CustDBGroupId";

    private readonly IDynamicRepository dynamicRepository = dynamicRepository;
    private readonly IMemoryCache memoryCache = memoryCache;

    public async Task<int> GetCustDBGroupId(int custId)
    {
        var siteDBConfig = await GetSiteDBConfig(custId);

        return siteDBConfig?.CustDBGroupId ?? 0;
    }

    public async Task<int> GetSiteDBGroupId(int custId)
    {
        var siteDBConfig = await GetSiteDBConfig(custId);

        return siteDBConfig?.SiteDBGroupId ?? 0;
    }

    public async Task<SiteDBConfig> GetSiteDBConfig(int custId)
    {
        var cacheKey = $"{SiteDBConfigCacheKey}_{custId}";

        if (!memoryCache.TryGetValue<SiteDBConfig>(cacheKey, out var siteDBConfig))
        {
            siteDBConfig = await dynamicRepository.GetAsync<SiteDBConfig>(new GetSiteDBConfigCriteria(custId));

            if (siteDBConfig != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(SiteDBConfigCacheInMinutes));
                memoryCache.Set(cacheKey, siteDBConfig, cacheEntryOptions);
            }
        }

        return siteDBConfig;
    }
}