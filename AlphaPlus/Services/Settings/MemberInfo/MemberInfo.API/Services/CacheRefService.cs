namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    using System.Threading;
    using Fanex.RemotingRef;

    public interface CacheRefService
    {
        string GetBetTypeName(int betTypeId);

        string GetProductGroupName(int productGroupId);

        string GetProductName(int productId);
        
        string GetSubGroupName(int productId);

        string GetSportTypeName(int sportTypeId);
    }

    public class CacheRefServiceImpl : CacheRefService
    {
        private readonly IRemotingRefService remotingRefService;
        private const string ProductRefId = "Product";
        private const string ProductGroupRefId = "ProductGroup";
        private const string SubGroupRefId = "SubGroup";
        private const string DummySportRefId = "DummySport";

        public CacheRefServiceImpl(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService;
        }

        public string GetBetTypeName(int betTypeId)
            => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

        public string GetProductGroupName(int productGroupId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(ProductGroupRefId, productGroupId.ToString(),
                LanguageCode, productGroupId.ToString());        
        
        public string GetSubGroupName(int subGroupRefId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(SubGroupRefId, subGroupRefId.ToString(),
                LanguageCode, string.Empty);
        
        public string GetProductName(int productId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                ProductRefId,
                productId.ToString(),
                LanguageCode,
                string.Empty);

        public string GetSportTypeName(int sportTypeId)
        {
            var sportTypeName = remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);
            if (string.IsNullOrWhiteSpace(sportTypeName))
            {
                sportTypeName = remotingRefService.GetResourceDataByRefIdAndResourceId(
                    DummySportRefId,
                    sportTypeId.ToString(),
                    LanguageCode,
                    string.Empty);
            }

            return sportTypeName;
        }

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}