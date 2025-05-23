using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AgencyCash.API.Payments.Services;

public interface IPaymentProviderConfigService
{
    Task<PaymentProviderConfig> GetConfig(PaymentProvider provider, int agentId);
}

public class PaymentProviderConfigService : IPaymentProviderConfigService
{
    private const int ConfigCacheInMinutes = 5;
    private const string ConfigCacheKey = "AgencyCash.API.PaymentProviderConfig";

    private readonly IDynamicRepository _repository;
    private readonly ISiteDBService _siteDBService;
    private readonly IMemoryCache _memoryCache;

    public PaymentProviderConfigService(
        IDynamicRepository repository,
        ISiteDBService siteDBService,
        IMemoryCache memoryCache)
    {
        _repository = repository;
        _siteDBService = siteDBService;
        _memoryCache = memoryCache;
    }

    public async Task<PaymentProviderConfig> GetConfig(PaymentProvider provider, int agentId)
    {
        var siteDBConfig = await _siteDBService.GetSiteDBConfig(agentId);
        var cacheKey = $"{ConfigCacheKey}_{provider}_{siteDBConfig.SiteDBGroupId}";

        if (!_memoryCache.TryGetValue<PaymentProviderConfig>(cacheKey, out var config))
        {
            config = await _repository.GetAsync<PaymentProviderConfig>(
                new GetPaymentProviderConfigCriteria(provider, siteDBConfig.SiteDBGroupId));

            if (config != null)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(ConfigCacheInMinutes));
                _memoryCache.Set(cacheKey, config, cacheEntryOptions);
            }
        }

        return config;
    }
} 