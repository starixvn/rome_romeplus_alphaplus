using Fanex.Caching;
using Fanex.Logging;

namespace AlphaPlus.NetThrottling.Utils
{
    public interface ICacheServiceWrapper
    {
        bool Contain(string key);

        void Set<T>(string key, T value);

        void Set<T>(string key, T value, CacheItemOptions options);

        bool TryGet<T>(string key, out T value);

        void Remove(string key);

        Task<bool> ContainAsync(string key);

        Task SetAsync<T>(string key, T value);

        Task SetAsync<T>(string key, T value, CacheItemOptions options);

        Task<(bool, T)> TryGetAsync<T>(string key);

        Task RemoveAsync(string key);
    }

    public class CacheServiceWrapper(ICacheService cacheService, ILogger logger) : ICacheServiceWrapper
    {
        private readonly ICacheService cacheService = cacheService;
        private readonly ILogger logger = logger;

        public bool Contain(string key)
        {
            try
            {
                return cacheService.Contain(key);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return false;
        }

        public void Set<T>(string key, T value)
        {
            try
            {
                cacheService.Set(key, value);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public void Set<T>(string key, T value, CacheItemOptions options)
        {
            try
            {
                cacheService.Set(key, value, options);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default(T);
            try
            {
                return cacheService.TryGet<T>(key, out value);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return false;
        }

        public void Remove(string key)
        {
            try
            {
                cacheService.Remove(key);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public async Task<bool> ContainAsync(string key)
        {
            try
            {
                return await cacheService.ContainAsync(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return false;
        }

        public async Task SetAsync<T>(string key, T value)
        {
            try
            {
                await cacheService.SetAsync<T>(key, value).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public async Task SetAsync<T>(string key, T value, CacheItemOptions options)
        {
            try
            {
                await cacheService.SetAsync<T>(key, value, options).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        public async Task<(bool, T)> TryGetAsync<T>(string key)
        {
            try
            {
                return await cacheService.TryGetAsync<T>(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }

            return (false, default(T));
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await cacheService.RemoveAsync(key).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
    }
}
