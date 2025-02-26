using Fanex.Caching;
using Fanex.Caching.Memory;
using Fanex.Caching.Redis;
using Fanex.Logging;

namespace AlphaPlus.NetThrottling.Utils
{
    public interface ICacheServiceFactory
    {
        ICacheServiceWrapper Create(RedisCacheOptions options);

        ICacheServiceWrapper Create();
    }

    public class CacheServiceFactory(IServiceProvider serviceProvider) : ICacheServiceFactory
    {
        private readonly Dictionary<string, ICacheServiceWrapper> cacheServices = [];
        private const string MemoryCacheKey = "MemoryCacheKey";
        private readonly IServiceProvider serviceProvider = serviceProvider;

        public ICacheServiceWrapper Create(RedisCacheOptions options)
        {
            if (!cacheServices.ContainsKey(options.ApplicationName))
            {
                lock (cacheServices)
                {
                    var cacheService = new CacheServiceWrapper(new CacheService(new RedisCache(options)), (ILogger)serviceProvider.GetService(typeof(ILogger)));
                    cacheServices.Add(options.ApplicationName, cacheService);
                    return cacheService;
                }
            }

            return cacheServices[options.ApplicationName];
        }

        public ICacheServiceWrapper Create()
        {
            if (!cacheServices.ContainsKey(MemoryCacheKey))
            {
                lock (cacheServices)
                {
                    var cacheService = new CacheServiceWrapper(new CacheService(new MemoryCache(new MemoryCacheOptions())), (ILogger)serviceProvider.GetService(typeof(ILogger)));
                    cacheServices.Add(MemoryCacheKey, cacheService);
                    return cacheService;
                }
            }

            return cacheServices[MemoryCacheKey];
        }
    }
}
