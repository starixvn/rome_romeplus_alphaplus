using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.ManagePayments;
using AlphaPlus.CashService.API.Models.ManagePayments;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IManagePaymentsService
{
    Task<IEnumerable<PaymentProviderModel>> GetAllPaymentProviders();
    Task<IEnumerable<PaymentProviderFeeModel>> GetPaymentProviderFees();
}

public class ManagePaymentsService : IManagePaymentsService
{
    private readonly IDynamicRepository _repository;

    public ManagePaymentsService(IDynamicRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PaymentProviderModel>> GetAllPaymentProviders()
    {
        // var result = await _repository.FetchAsync<PaymentProviderModel>(new GetAllPaymentProvidersCriteria());
        return await GetDummyProviders();
    }
    
    public async Task<IEnumerable<PaymentProviderFeeModel>> GetPaymentProviderFees()
    {
        // var result = await _repository.FetchAsync<PaymentProviderFeeModel>(new GetPaymentProvidersFeesCriteria());
        return await GetDummyProviderFees();
    }

    private async Task<IEnumerable<PaymentProviderFeeModel>> GetDummyProviderFees()
    {
        return new List<PaymentProviderFeeModel>
        {
            new PaymentProviderFeeModel
            {
                TransactionType = "Deposit",
                Method = "Credit Card",
                Rate = 2.5m
            },
            new PaymentProviderFeeModel
            {
                TransactionType = "Withdrawal",
                Method = "Bank Transfer",
                Rate = 1.5m
            },
            new PaymentProviderFeeModel
            {
                TransactionType = "Deposit",
                Method = "E-Wallet",
                Rate = 1.0m
            },
            new PaymentProviderFeeModel
            {
                TransactionType = "Withdrawal",
                Method = "Cryptocurrency",
                Rate = 3.0m
            },
            new PaymentProviderFeeModel
            {
                TransactionType = "Deposit",
                Method = "PayPal",
                Rate = 2.0m
            }
        };
    }
    
    public async Task<IEnumerable<PaymentProviderModel>> GetDummyProviders()
    {
        return new List<PaymentProviderModel>
        {
            new PaymentProviderModel
            {
                Id = 1,
                Name = "Credit Card",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-1)
            },
            new PaymentProviderModel
            {
                Id = 2,
                Name = "Bank Transfer",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-2)
            },
            new PaymentProviderModel
            {
                Id = 3,
                Name = "E-Wallet",
                Status = PaymentProviderStatusConstants.Inactive,
                LastUpdated = DateTime.Now.AddDays(-5)
            },
            new PaymentProviderModel
            {
                Id = 4,
                Name = "Cryptocurrency",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-3)
            },
            new PaymentProviderModel
            {
                Id = 5,
                Name = "PayPal",
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-1)
            }
        };
    }
} 