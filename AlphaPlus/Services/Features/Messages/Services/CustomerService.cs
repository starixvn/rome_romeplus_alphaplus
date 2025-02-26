using AlphaPlus.Features.Messages.API.Criteria.Customer;
using Fanex.Data.Repository;
using AlphaPlus.Features.Messages.API.Models.Customer;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface CustomerService
    {
        Customer GetCustomerInfo(int custId);

        bool IsAllDayBalanceTransfer(int custId);
    }

    public class CustomerServiceImpl : CustomerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public CustomerServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public Customer GetCustomerInfo(int custId)
            => dynamicRepository.Get<Customer>(new GetCustomerInfoCriteria(custId));

        public bool IsAllDayBalanceTransfer(int custId)
        {
            var customerTransferStatus = dynamicRepository.Get<CustomerTransferStatus>(
                new GetIsAllDayBalTransferCriteria(custId));

            return customerTransferStatus != null && customerTransferStatus.IsAllDayBalTransfer;
        }
    }
}