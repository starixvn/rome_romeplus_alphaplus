namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    using Criteria;
    using Fanex.Data.Repository;
    using Models;

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public CustomerInfo GetDownlineInfoByUsername(string downlineUsername, int uplineId)
        {
            var criteria = new GetCustInfoByUsernameCommand
            {
                Username = downlineUsername,
                UplineId = uplineId
            };

            dynamicRepository.Execute(criteria);

            var customerInfo = new CustomerInfo(criteria.CustId, criteria.IsSubAcc, criteria.RoleId);

            return customerInfo;
        }
    }
}
