using AlphaPlus.Core.Configurations;
using Fanex.RemotingRef;

namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    public interface CacheRefService
    {
        string GetBetTypeName(int betTypeId);

        string GetProductGroupName(int productGroupId);

        string GetProductName(int productId);

        string GetSubGroupName(int productId);

        string GetSportTypeName(int sportTypeId);

        string GetCustomerProfileStatus(int categoryID);

        string GetCustomerProfileLevel(int categoryID);

        string GetCustBetBehavior(string id);
    }

    public class CacheRefServiceImpl : CacheRefService
    {
        private readonly IRemotingRefService remotingRefService;
        private const string ProductRefId = "Product";
        private const string ProductGroupRefId = "ProductGroup";
        private const string SubGroupRefId = "SubGroup";
        private const string DummySportRefId = "DummySport";

        private const string CustomerProfileStatus = "CategoryStatus";
        private const string CustomerProfileLevel = "CategoryLevel";
        private const string CustBetBehavior = "CustBetBehaviour";

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

        public string GetCustomerProfileStatus(int categoryID)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(CustomerProfileStatus, categoryID.ToString(), LanguageCode, "N/A");

        public string GetCustomerProfileLevel(int categoryID)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(CustomerProfileLevel, categoryID.ToString(), LanguageCode, "N/A");

        public string GetCustBetBehavior(string id)
        {
            if (AppSettings.IsSiteLabel777)
            {
                var custBetBehavior777Name = remotingRefService.GetResourceDataByRefIdAndResourceId(
                    CustBetBehavior, $"{id}_{AppSettings.SiteLabel}", LanguageCode, string.Empty);

                return string.IsNullOrEmpty(custBetBehavior777Name)
                    ? remotingRefService.GetResourceDataByRefIdAndResourceId(CustBetBehavior, id, LanguageCode, string.Empty)
                    : custBetBehavior777Name;
            }

            return remotingRefService.GetResourceDataByRefIdAndResourceId(CustBetBehavior, id, LanguageCode, string.Empty);
        }

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}