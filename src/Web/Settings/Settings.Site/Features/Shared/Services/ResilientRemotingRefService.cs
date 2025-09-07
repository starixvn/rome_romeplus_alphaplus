using Fanex.RemotingRef;
using Polly;
using Microsoft.Extensions.Logging;
using Polly.Retry;

namespace AlphaPlus.Settings.Site.Features.Shared.Services
{
    public class ResilientRemotingRefService : ICacheRefService
    {
        private readonly ICacheRefService baseCacheRefService;
        private readonly ILogger<ResilientRemotingRefService> logger;
        
        // Create resilience pipeline with retry 3 times and 100ms timeout
        private static readonly ResiliencePipeline resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder()
                    .Handle<Exception>(ex => !(ex is ArgumentException || ex is ArgumentNullException)),
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(50), // Base delay between retries
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true
            })
            .AddTimeout(TimeSpan.FromMilliseconds(100)) // 100ms timeout
            .Build();

        public ResilientRemotingRefService(ICacheRefService baseCacheRefService, ILogger<ResilientRemotingRefService> logger)
        {
            this.baseCacheRefService = baseCacheRefService;
            this.logger = logger;
        }

        public string GetBetTypeName(int betTypeId)
        {
            return ExecuteWithResilience(() => 
                baseCacheRefService.GetBetTypeName(betTypeId),
                $"GetBetTypeName-{betTypeId}",
                string.Empty);
        }

        public string GetProductName(int productId)
        {
            return ExecuteWithResilience(() => 
                baseCacheRefService.GetProductName(productId),
                $"GetProductName-{productId}",
                string.Empty);
        }

        public string GetProductName(int productId, string languageCode)
        {
            return ExecuteWithResilience(() => 
                baseCacheRefService.GetProductName(productId, languageCode),
                $"GetProductName-{productId}-{languageCode}",
                string.Empty);
        }

        public string GetSportTypeName(int sportTypeId)
        {
            return ExecuteWithResilience(() => 
                baseCacheRefService.GetSportTypeName(sportTypeId),
                $"GetSportTypeName-{sportTypeId}",
                string.Empty);
        }

        public string GetSportTypeName(int sportTypeId, string languageCode)
        {
            return ExecuteWithResilience(() => 
                baseCacheRefService.GetSportTypeName(sportTypeId, languageCode),
                $"GetSportTypeName-{sportTypeId}-{languageCode}",
                string.Empty);
        }

        public string GetProductGroupName(int productGroupId)
        {
            return ExecuteWithResilience(() => 
                baseCacheRefService.GetProductGroupName(productGroupId),
                $"GetProductGroupName-{productGroupId}",
                string.Empty);
        }

        private string ExecuteWithResilience(Func<string> operation, string operationName, string fallbackValue)
        {
            try
            {
                return resiliencePipeline.Execute(operation);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "CacheRef operation {OperationName} failed after 3 retries within 100ms timeout, returning fallback: {FallbackValue}", 
                    operationName, fallbackValue);
                return fallbackValue ?? string.Empty;
            }
        }
    }
}