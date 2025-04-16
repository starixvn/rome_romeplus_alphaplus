using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.ManagePayments;
using AlphaPlus.CashService.API.Models.ManagePayments;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IManagePaymentsService
{
    Task<IEnumerable<PaymentProviderModel>> GetAllPaymentProviders();
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
        var result = await _repository.FetchAsync<PaymentProviderModel>(new GetAllPaymentProvidersCriteria());
        return await GetDummyProviders();
    }
    
    public async Task<IEnumerable<PaymentProviderModel>> GetDummyProviders()
    {
        // Return dummy payment provider data
        return new List<PaymentProviderModel>
        {
            new PaymentProviderModel
            {
                Id = 1,
                Name = "Credit Card",
                Fee = 2.5m,
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-1)
            },
            new PaymentProviderModel
            {
                Id = 2,
                Name = "Bank Transfer",
                Fee = 1.0m,
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-2)
            },
            new PaymentProviderModel
            {
                Id = 3,
                Name = "E-Wallet",
                Fee = 1.5m,
                Status = PaymentProviderStatusConstants.Inactive,
                LastUpdated = DateTime.Now.AddDays(-5)
            },
            new PaymentProviderModel
            {
                Id = 4,
                Name = "Cryptocurrency",
                Fee = 3.0m,
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-3)
            },
            new PaymentProviderModel
            {
                Id = 5,
                Name = "PayPal",
                Fee = 2.0m,
                Status = PaymentProviderStatusConstants.Active,
                LastUpdated = DateTime.Now.AddDays(-1)
            }
        };
    }
} 