using AlphaPlus.NetThrottling.Filters;
using AlphaPlus.NetThrottling.Models;
using AlphaPlus.NetThrottling.Resolvers;
using AlphaPlus.NetThrottling.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

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
    }

    public class NetThrottler : INetThrottler
    {
        private readonly IClock clock;
        public IResolver Resolver { get; private set; }

        public ThrottlePolicy[] Policies { get; private set; }

        private readonly NetThrottlingConfiguration configuration;

        private readonly IMemoryCache cacheService;


        public NetThrottler(IOptions<NetThrottlingConfiguration> options, IMemoryCache cacheService, IClock clock, IResolverFactory resolverFactory)
        {
            configuration = options.Value;
            this.clock = clock;
            Resolver = resolverFactory.GetResolver(configuration.Resolver);
            Policies = configuration.Policies;
            this.cacheService = cacheService;
        }

        public bool IsBlacklist(CacheIdentity cacheIdentity)
        {
            return cacheService.TryGetValue(cacheIdentity.BlockKey, out _);
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
                clock.UtcNow,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(policy.BlockDuration)
                });
        }

        public TimeSpan GetBlockRemainingTime(ThrottlePolicy policy, CacheIdentity cacheIdentity)
        {
            var isHasValue = cacheService.TryGetValue(cacheIdentity.BlockKey, out DateTime blockTime);
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

            var isHasValue = cacheService.TryGetValue(cacheIdentity.ThrottleKey, out ThrottleCounter throttleCounter);
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
                throttleCounter, new MemoryCacheEntryOptions
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
    }
}
