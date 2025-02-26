using AlphaPlus.PartialUM.Services;
using AlphaPlus.Settings.MemberInfo.API.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile
{
    public interface ICustomerProfileService
    {
        Task<CustomerProfile> GetCustomerProfile(int custId, string siteName);
    }

    public class CustomerProfileService : ICustomerProfileService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly CacheRefService refService;
        private readonly IPartialUMService partialUMService;
        private readonly HttpClient httpClient;
        private readonly Fanex.Logging.ILogger logger;

        public CustomerProfileService(
            IDynamicRepository dynamicRepository,
            IPartialUMService partialUMService,
            CacheRefService refService,
            HttpClient httpClient,
            Fanex.Logging.ILogger logger)
        {
            this.dynamicRepository = dynamicRepository;
            this.partialUMService = partialUMService;
            this.refService = refService;
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<CustomerProfile> GetCustomerProfile(int custId, string siteName)
        {
            var criteria = new GetCustomerProfileCriteria(custId);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

            var customerProfile = await dynamicRepository.GetAsync<CustomerProfile>(criteria);

            if (customerProfile != null)
            {
                customerProfile.CategoryID = await GetCTSCategoryId(custId, customerProfile.CustomerClass);
                customerProfile.Status = refService.GetCustomerProfileStatus(customerProfile.CategoryID);
                customerProfile.Level = refService.GetCustomerProfileLevel(customerProfile.CategoryID);
                customerProfile.RelatedToOtherAccount = customerProfile.IsProblemAccount;
                if (!customerProfile.RelatedToOtherAccount)
                {
                    customerProfile.RelatedToOtherAccount = await GetCTSIsAssociated(custId);
                }
            }

            return customerProfile;
        }

        private async Task<bool> GetCTSIsAssociated(int custId)
        {
            try
            {
                var customerAssociation = await httpClient.GetAsJsonAsync<CustomerAssociation>($"cts/Association/CheckByCustId?custId={custId}");

                return customerAssociation.IsAssociated;
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
            }

            return false;
        }

        private async Task<int> GetCTSCategoryId(int custId, int? customerClass)
        {
            try
            {
                var result = await httpClient.GetAsJsonAsync<CTSCustomerClassCategoryResult>($"cts/GetCustomerClassCategoryByCustID?custid={custId}");

                if (result?.Detail?.Count() > 0)
                {
                    var customerClassCategory = result.Detail
                        .OrderByDescending(c => c.Date)
                        .FirstOrDefault(c => c.CC == (customerClass ?? 0)) ?? result.Detail.First();

                    return customerClassCategory.CategoryId;
                }
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(ex.Message, ex);
            }

            return 0;
        }
    }
}