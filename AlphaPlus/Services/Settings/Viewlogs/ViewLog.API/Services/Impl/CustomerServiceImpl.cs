using AlphaPlus.Settings.ViewLogs.API.Criteria;
using Fanex.Data.Repository;
using AlphaPlus.Settings.ViewLogs.API.Models;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class CustomerServiceImpl(IDynamicRepository dynamicRepository) : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository = dynamicRepository;

        public async Task<CustomerInfo> GetDownlineInfoByUsername(string downlineUsername, int uplineId)
        {
            var criteria = new GetCustInfoByUsernameCommand
            {
                Username = downlineUsername,
                UplineId = uplineId
            };

            await dynamicRepository.ExecuteAsync(criteria);

            var customerInfo = new CustomerInfo(criteria.CustId, criteria.IsSubAcc, criteria.RoleId);

            return customerInfo;
        }
    }
}
