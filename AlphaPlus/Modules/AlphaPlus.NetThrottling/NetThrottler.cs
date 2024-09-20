using AlphaPlus.NetThrottling.Filters;
using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Resolvers;
using AlphaPlus.NetThrottling.Utils;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace AlphaPlus.NetThrottling
{
    public interface INetThrottler
    {
        IResolver Resolver { get; }

        ThrottlePolicy[] Policies { get; }

        bool IsBlacklist(CacheIdentity cacheIdentity);

        void RemoveBlacklist(CacheIdentity cacheIdentity);

        void RemoveThrottling(CacheIdentity cacheIdentity);

        void AddBlacklist(ThrottlePolicy policy, CacheIdentity cacheIdentity);

        TimeSpan GetBlockRemainingTime(ThrottlePolicy policy, CacheIdentity cacheIdentity);

        ThrottleResult Throttle(HttpContext httpContext, ThrottlePolicy policy, IFilter filter);

        ThrottleResult Throttle(HttpContext httpContext, ThrottlePolicy policy, IFilter filter, bool blockImmediately);

        ThrottleResult Throttle(ThrottlePolicy policy, CacheIdentity cacheIdentity);

        ThrottleResult Throttle(ThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately);

        TResult Throttle<TResult>(
            HttpContext httpContext,
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            IFilter filter);

        TResult Throttle<TResult>(
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            CacheIdentity cacheIdentity);
        Task<bool> IsBlacklistAsync(CacheIdentity cacheIdentity);

        Task RemoveBlacklistAsync(CacheIdentity cacheIdentity);

        Task RemoveThrottlingAsync(CacheIdentity cacheIdentity);

        Task AddBlacklistAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<TimeSpan> GetBlockRemainingTimeAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, ThrottlePolicy policy, IFilter filter);

        Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, ThrottlePolicy policy, IFilter filter, bool blockImmediately);

        Task<ThrottleResult> ThrottleAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity);

        Task<ThrottleResult> ThrottleAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately);

        Task<TResult> ThrottleAsync<TResult>(
            HttpContext httpContext,
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            IFilter filter);

        Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            CacheIdentity cacheIdentity);
    }

    public class NetThrottler : INetThrottler
    {
        private readonly IClock clock;
        public IResolver Resolver { get; private set; }

        public ThrottlePolicy[] Policies { get; private set; }

        private readonly NetThrottlingConfiguration configuration;

        private readonly ICacheServiceWrapper cacheService;


        public NetThrottler(IOptions<NetThrottlingConfiguration> options, ICacheServiceFactory cacheServiceFactory, IClock clock)
        {
            configuration = options.Value;
            this.clock = clock;
            Resolver = string.IsNullOrEmpty(configuration.Resolver) ? new BaseResolver() : TypeFactory<IResolver>.Create(configuration.Resolver);
            Policies = configuration.Policies;

            if (configuration.IsUseRedis)
            {
                var configurationOptions = ConfigurationOptions.Parse(configuration.RedisConfigurationOptions);
                var redisCacheOptions = new RedisCacheOptions
                {
                    ApplicationName = configurationOptions.ClientName,
                    ConnectionOptions = configurationOptions
                };
                cacheService = cacheServiceFactory.Create(redisCacheOptions);
            }
            else
            {
                cacheService = cacheServiceFactory.Create();
            }
        }

        public bool IsBlacklist(CacheIdentity cacheIdentity)
        {
            return cacheService.Contain(cacheIdentity.BlockKey);
        }

        public void RemoveBlacklist(CacheIdentity cacheIdentity)
        {
            cacheService.Remove(cacheIdentity.BlockKey);
        }

        public void RemoveThrottling(CacheIdentity cacheIdentity)
        {
            cacheService.Remove(cacheIdentity.ThrottleKey);
        }

        public void AddBlacklist(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            cacheService.Set(
                cacheIdentity.BlockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(policy.BlockDuration)
                });
        }

        public TimeSpan GetBlockRemainingTime(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            var isHasValue = cacheService.TryGet(cacheIdentity.BlockKey, out DateTime blockTime);
            if (isHasValue)
            {
                return blockTime.AddSeconds(policy.BlockDuration) - clock.UtcNow;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public ThrottleResult Throttle(HttpContext httpContext, ThrottlePolicy policy, IFilter filter)
        {
            return Throttle(httpContext, policy, filter, false);
        }

        public ThrottleResult Throttle(HttpContext httpContext, ThrottlePolicy policy, IFilter filter, bool blockImmediately)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return Throttle(policy, cacheIdentity, blockImmediately);
        }
        public ThrottleResult Throttle(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            return Throttle(policy, cacheIdentity, false);
        }

        public ThrottleResult Throttle(ThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately)
        {
            if (cacheIdentity == null)
                return ThrottleResult.Allowed;
            if (IsBlacklist(cacheIdentity))
            {
                return ThrottleResult.Blocked;
            }

            var isHasValue = cacheService.TryGet(cacheIdentity.ThrottleKey, out ThrottleCounter throttleCounter);
            throttleCounter = !isHasValue || clock.UtcNow > throttleCounter.Timestamp + TimeSpan.FromSeconds(policy.ThrottleDuration)
                ? new ThrottleCounter
                {
                    Timestamp = clock.UtcNow,
                    TotalRequests = 1
                }
                : new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };

            if (throttleCounter.TotalRequests > policy.MaxRequests ||
                (blockImmediately && throttleCounter.TotalRequests == policy.MaxRequests))
            {
                AddBlacklist(policy, cacheIdentity);
                RemoveThrottling(cacheIdentity);
                return ThrottleResult.Blocked;
            }

            cacheService.Set(
                cacheIdentity.ThrottleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.ThrottleDuration)
                });
            return ThrottleResult.Allowed;
        }

        public TResult Throttle<TResult>(
            HttpContext httpContext,
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return Throttle(work, blocked, policy, cacheIdentity);
        }

        public TResult Throttle<TResult>(
            Func<Func<TResult, TResult>, TResult> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            TResult success(TResult returnType)
            {
                var throttleResult = Throttle(policy, cacheIdentity);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    AddBlacklist(policy, cacheIdentity);
                    RemoveThrottling(cacheIdentity);
                    return blocked();
                }

                return returnType;
            }

            if (cacheIdentity == null)
            {
                return work(success);
            }

            if (IsBlacklist(cacheIdentity))
            {
                return blocked();
            }

            return work(success);
        }

        public async Task<bool> IsBlacklistAsync(CacheIdentity cacheIdentity)
        {
            return await cacheService.ContainAsync(cacheIdentity.BlockKey).ConfigureAwait(false);
        }

        public async Task RemoveBlacklistAsync(CacheIdentity cacheIdentity)
        {
            await cacheService.RemoveAsync(cacheIdentity.BlockKey).ConfigureAwait(false);
        }

        public async Task RemoveThrottlingAsync(CacheIdentity cacheIdentity)
        {
            await cacheService.RemoveAsync(cacheIdentity.ThrottleKey).ConfigureAwait(false);
        }

        public async Task<TimeSpan> GetBlockRemainingTimeAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            var (isHasValue, blockTime) = await cacheService.TryGetAsync<DateTime>(cacheIdentity.BlockKey).ConfigureAwait(false);
            if (isHasValue)
            {
                return blockTime.AddSeconds(policy.BlockDuration) - clock.UtcNow;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }

        public async Task AddBlacklistAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            await cacheService.SetAsync(
                cacheIdentity.BlockKey,
                clock.UtcNow, new CacheItemOptions
                {
                    RelativeExpiration = TimeSpan.FromSeconds(policy.BlockDuration)
                }).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, ThrottlePolicy policy, IFilter filter)
        {
            return await ThrottleAsync(httpContext, policy, filter, false).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(HttpContext httpContext, ThrottlePolicy policy, IFilter filter, bool blockImmediately)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            return await ThrottleAsync(policy, cacheIdentity, false).ConfigureAwait(false);
        }

        public async Task<ThrottleResult> ThrottleAsync(ThrottlePolicy policy, CacheIdentity cacheIdentity, bool blockImmediately)
        {
            if (cacheIdentity == null)
            {
                return ThrottleResult.Allowed;
            }

            if (await IsBlacklistAsync(cacheIdentity).ConfigureAwait(false))
            {
                return ThrottleResult.Blocked;
            }

            var (isHasValue, throttleCounter) = await cacheService.TryGetAsync<ThrottleCounter>(cacheIdentity.ThrottleKey).ConfigureAwait(false);
            throttleCounter = !isHasValue || clock.UtcNow > throttleCounter.Timestamp + TimeSpan.FromSeconds(policy.ThrottleDuration)
                ? new ThrottleCounter
                {
                    Timestamp = clock.UtcNow,
                    TotalRequests = 1
                }
                : new ThrottleCounter
                {
                    Timestamp = throttleCounter.Timestamp,
                    TotalRequests = throttleCounter.TotalRequests + 1
                };

            if (throttleCounter.TotalRequests > policy.MaxRequests ||
                (blockImmediately && throttleCounter.TotalRequests == policy.MaxRequests))
            {
                await AddBlacklistAsync(policy, cacheIdentity).ConfigureAwait(false);
                await RemoveThrottlingAsync(cacheIdentity).ConfigureAwait(false);
                return ThrottleResult.Blocked;
            }

            await cacheService.SetAsync(
                cacheIdentity.ThrottleKey,
                throttleCounter, new CacheItemOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.ThrottleDuration)
                }).ConfigureAwait(false);
            return ThrottleResult.Allowed;
        }
        public async Task<TResult> ThrottleAsync<TResult>(
            HttpContext httpContext,
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            IFilter filter)
        {
            var cacheIdentity = filter.GetCacheIdentity(httpContext, policy);
            return await ThrottleAsync(work, blocked, policy, cacheIdentity).ConfigureAwait(false);
        }

        public async Task<TResult> ThrottleAsync<TResult>(
            Func<Func<TResult, Task<TResult>>, Task<TResult>> work,
            Func<TResult> blocked,
            ThrottlePolicy policy,
            CacheIdentity cacheIdentity)
        {
            async Task<TResult> success(TResult returnType)
            {
                var throttleResult = await ThrottleAsync(policy, cacheIdentity).ConfigureAwait(false);
                if (throttleResult == ThrottleResult.Blocked)
                {
                    await AddBlacklistAsync(policy, cacheIdentity).ConfigureAwait(false);
                    await RemoveThrottlingAsync(cacheIdentity).ConfigureAwait(false);
                    return blocked();
                }

                return returnType;
            }

            if (cacheIdentity == null)
            {
                return await work(success).ConfigureAwait(false);
            }

            if (await IsBlacklistAsync(cacheIdentity).ConfigureAwait(false))
            {
                return blocked();
            }

            return await work(success).ConfigureAwait(false);
        }
    }
}
