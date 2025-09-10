using Fanex.RemotingRef;
using Polly;
using Polly.Simmy;

namespace AlphaPlus.Settings.Site.Features.Shared.Services
{
    public interface ICacheRefService
    {
        string GetBetTypeName(int betTypeId);

        string GetProductName(int productId);

        string GetProductName(int productId, string languageCode);

        string GetSportTypeName(int sportTypeId);

        string GetSportTypeName(int sportTypeId, string languageCode);

        string GetProductGroupName(int productGroupId);
    }

    public class CacheRefService(IRemotingRefService remotingRefService) : ICacheRefService
    {
        private readonly IRemotingRefService remotingRefService = remotingRefService;

        // 🎲 Chaos Engineering - 50% chance of 10s delay (only when NOT Development)
        private static readonly ResiliencePipeline chaosStrategy = new ResiliencePipelineBuilder()
            .AddChaosLatency(new Polly.Simmy.Latency.ChaosLatencyStrategyOptions
            {
                EnabledGenerator = args => ValueTask.FromResult(true), // Enabled when NOT Dev
                InjectionRateGenerator = args => ValueTask.FromResult(1d), // 50% chance
                LatencyGenerator = args => ValueTask.FromResult(TimeSpan.FromSeconds(10)) // 10 seconds delay
            })
            .Build();

        public string GetBetTypeName(int betTypeId)
            => chaosStrategy.Execute(() => 
                remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode));

        public string GetProductName(int productId)
            => chaosStrategy.Execute(() => 
                GetProductName(productId, LanguageCode));

        public string GetProductName(int productId, string languageCode)
            => chaosStrategy.Execute(() => 
                remotingRefService.GetResourceDataByRefIdAndResourceId(
                    "Product",
                    productId.ToString(),
                    languageCode,
                    string.Empty));

        public string GetSportTypeName(int sportTypeId, string languageCode)
            => chaosStrategy.Execute(() => 
                remotingRefService.GetSportName(sportTypeId.ToString(), languageCode, null) ?? 
                remotingRefService.GetResourceDataByRefIdAndResourceId(
                    "DummySport",
                    sportTypeId.ToString(),
                    languageCode,
                    string.Empty));

        public string GetSportTypeName(int sportTypeId)
            => chaosStrategy.Execute(() => 
                remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode, null) ?? 
                remotingRefService.GetResourceDataByRefIdAndResourceId(
                    "DummySport",
                    sportTypeId.ToString(),
                    LanguageCode,
                    string.Empty));

        public string GetProductGroupName(int productGroupId)
            => chaosStrategy.Execute(() => 
                remotingRefService.GetResourceDataByRefIdAndResourceId("ProductGroup", productGroupId.ToString(),
                    LanguageCode, productGroupId.ToString()));

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;

        // Check if current environment is Development
        private static bool IsDevelopment
            => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.Equals("Development", StringComparison.OrdinalIgnoreCase) == true;
    }
}