using Fanex.RemotingRef;

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

        public string GetBetTypeName(int betTypeId)
            => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public string GetProductName(int productId)
            => GetProductName(productId, LanguageCode);

        public string GetProductName(int productId, string languageCode)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                languageCode,
                string.Empty);

        public string GetSportTypeName(int sportTypeId, string languageCode)
            => remotingRefService.GetSportName(sportTypeId.ToString(), languageCode, null) ?? remotingRefService.GetResourceDataByRefIdAndResourceId(
                "DummySport",
                sportTypeId.ToString(),
                languageCode,
                string.Empty);

        public string GetSportTypeName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode, null) ?? remotingRefService.GetResourceDataByRefIdAndResourceId(
                "DummySport",
                sportTypeId.ToString(),
                LanguageCode,
                string.Empty);

        public string GetProductGroupName(int productGroupId)
         => remotingRefService.GetResourceDataByRefIdAndResourceId("ProductGroup", productGroupId.ToString(),
                LanguageCode, productGroupId.ToString());

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}