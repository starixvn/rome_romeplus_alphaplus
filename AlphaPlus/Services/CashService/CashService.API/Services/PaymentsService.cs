using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<PaymentProviderModel>> GetAllPaymentProviders();
    Task<IEnumerable<PaymentProviderFeeGroupModel>> GetPaymentProviderFees(int providerId);
    Task<ResultModel> SetDailyWithdrawalCount(int custId, int count);
    Task<IEnumerable<PaymentProviderSettingsModel>> GetPaymentProviderSettings(int providerId, int method, int status, int transactionType);
}

public class PaymentsService : IPaymentsService
{
    private readonly IDynamicRepository _repository;

    public PaymentsService(IDynamicRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PaymentProviderModel>> GetAllPaymentProviders()
    {
        // var result = await _repository.FetchAsync<PaymentProviderModel>(new GetAllPaymentProvidersCriteria());
        var providers = await GetDummyProviders();
        
        return providers;
    }
    
    public async Task<IEnumerable<PaymentProviderFeeGroupModel>> GetPaymentProviderFees(int providerId)
    {
        // var result = await _repository.FetchAsync<PaymentProviderFeeModel>(new GetPaymentProvidersFeesCriteria(providerId));
        var fees = await GetDummyProviderFees(providerId);
        
        var groupedFees = fees
            .GroupBy(f => f.TransactionType)
            .Select(g => new PaymentProviderFeeGroupModel
            {
                TransactionType = g.Key,
                Fees = g.ToList()
            })
            .ToList();
        
        return groupedFees;
    }

    public async Task<IEnumerable<PaymentProviderSettingsModel>> GetPaymentProviderSettings(int providerId, int method, int status, int transactionType)
    {
        // var result = await _repository.FetchAsync<PaymentProviderSettingsModel>(new GetPaymentProviderSettingsCriteria(providerId, method, status, transactionType));
        var settings = await GetDummyProviderSettings(providerId, method, status, transactionType);
        
        return settings;
    }

    private async Task<IEnumerable<PaymentProviderSettingsModel>> GetDummyProviderSettings(int providerId, int method, int status, int transactionType)
    {
        return new List<PaymentProviderSettingsModel>
        {
            new PaymentProviderSettingsModel
            {
                ProviderId = providerId,
                Method = method,
                Status = status,
                TransactionType = transactionType,
                MinAmount = 10.00m,
                MaxAmount = 1000.00m,
                IsEnabled = true,
                BankRequired = false
            },
            new PaymentProviderSettingsModel
            {
                ProviderId = providerId,
                Method = method,
                Status = status,
                TransactionType = transactionType,
                MinAmount = 20.00m,
                MaxAmount = 2000.00m,
                IsEnabled = true,
                BankRequired = true
            }
        };
    }

    private async Task<IEnumerable<PaymentProviderFeeModel>> GetDummyProviderFees(int providerId)
    {
        return new List<PaymentProviderFeeModel>
        {
            new PaymentProviderFeeModel
            {
                ProviderId = 1,
                TransactionType = "Deposit",
                Method = "Credit Card",
                Rate = 2.5m
            },
            new PaymentProviderFeeModel
            {
                ProviderId = 2,
                TransactionType = "Withdrawal",
                Method = "Bank Transfer",
                Rate = 1.5m
            },
            new PaymentProviderFeeModel
            {
                ProviderId = 3,
                TransactionType = "Deposit",
                Method = "E-Wallet",
                Rate = 1.0m
            },
            new PaymentProviderFeeModel
            {
                ProviderId = 4,
                TransactionType = "Withdrawal",
                Method = "Cryptocurrency",
                Rate = 3.0m
            },
            new PaymentProviderFeeModel
            {
                ProviderId = 5,
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

    public async Task<ResultModel> SetDailyWithdrawalCount(int custId, int count)
    {
        try
        {
            // TODO: Implement actual database update logic
            // For now returning dummy implementation
            // var result = await _repository.ExecuteNonQueryAsync(new SetDailyWithdrawalCountCriteria(custId, count));
            return new ResultModel("0", "Success");
        }
        catch (Exception ex)
        {
            return new ResultModel("1", ex.Message);
        }
    }
} 