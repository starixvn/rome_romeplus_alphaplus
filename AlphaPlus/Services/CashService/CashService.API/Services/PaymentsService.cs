using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetAllPaymentProviders();
    Task<ProviderModel> GetPaymentProviderById(int providerId);
    Task<IEnumerable<ProviderFeeGroupModel>> GetPaymentProviderFees(int providerId);
    Task<ResultModel> SetDailyWithdrawalCount(int custId, int count);
    Task<IEnumerable<ProviderSettingsModel>> GetPaymentProviderSettings(int providerId, int method, int status, int transactionType);
    Task<ResultModel> UpdatePaymentProviderSettings(IEnumerable<ProviderSettingsModel> settings);
    Task<ResultModel> AddPaymentProviderSetting(ProviderSettingsModel setting);
    Task<IEnumerable<PaymentMethodModel>> GetAllPaymentMethods();
    Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod);
}

public class PaymentsService : IPaymentsService
{
    private readonly IDynamicRepository _repository;

    public PaymentsService(IDynamicRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProviderModel>> GetAllPaymentProviders()
    {
        // var result = await _repository.FetchAsync<ProviderModel>(new GetAllPaymentProvidersCriteria());
        var providers = await DummyDataService.GetDummyProviders();
        
        return providers;
    }
    
    public async Task<ProviderModel> GetPaymentProviderById(int providerId)
    {
        // var result = await _repository.FetchAsync<ProviderModel>(new GetPaymentProviderByIdCriteria(providerId));
        var providers = await DummyDataService.GetDummyProviders();
        var provider = providers.FirstOrDefault(p => p.Id == providerId);
        
        return provider;
    }
    
    public async Task<IEnumerable<ProviderFeeGroupModel>> GetPaymentProviderFees(int providerId)
    {
        // var result = await _repository.FetchAsync<ProviderFeeModel>(new GetPaymentProvidersFeesCriteria(providerId));
        var fees = await DummyDataService.GetDummyProviderFees(providerId);
        
        var groupedFees = fees
            .GroupBy(f => f.TransactionType)
            .Select(g => new ProviderFeeGroupModel
            {
                TransactionType = g.Key,
                Fees = g.ToList()
            })
            .ToList();
        
        return groupedFees;
    }

    public async Task<IEnumerable<ProviderSettingsModel>> GetPaymentProviderSettings(int providerId, int method, int status, int transactionType)
    {
        // var result = await _repository.FetchAsync<ProviderSettingsModel>(new GetPaymentProviderSettingsCriteria(providerId, method, status, transactionType));
        var settings = await DummyDataService.GetDummyProviderSettings(providerId, method, status, transactionType);
        
        return settings;
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

    public async Task<ResultModel> UpdatePaymentProviderSettings(IEnumerable<ProviderSettingsModel> settings)
    {
        try
        {
            if (settings == null || !settings.Any())
            {
                return new ResultModel("1", "No settings provided");
            }

            foreach (var setting in settings)
            {                
                // TODO: Implement actual database update logic for each setting
                // var result = await _repository.ExecuteNonQueryAsync(new UpdatePaymentProviderSettingCriteria(setting));
            }

            return new ResultModel("0", "Success");
        }
        catch (Exception ex)
        {
            return new ResultModel("1", ex.Message);
        }
    }

    public async Task<ResultModel> AddPaymentProviderSetting(ProviderSettingsModel setting)
    {
        try
        {
            if (setting == null)
            {
                return new ResultModel("1", "No setting provided");
            }

            // TODO: Implement actual database insert logic
            // var result = await _repository.ExecuteNonQueryAsync(new AddPaymentProviderSettingCriteria(setting));

            return new ResultModel("0", "Success");
        }
        catch (Exception ex)
        {
            return new ResultModel("1", ex.Message);
        }
    }

    public async Task<IEnumerable<PaymentMethodModel>> GetAllPaymentMethods()
    {
        // TODO: Implement actual database query
        // var result = await _repository.FetchAsync<PaymentMethodModel>(new GetAllPaymentMethodsCriteria());
        var methods = await DummyDataService.GetDummyPaymentMethods();
        
        return methods;
    }

    public async Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod)
    {
        try
        {
            if (paymentMethod == null)
            {
                return new ResultModel("1", "No payment method provided");
            }

            // TODO: Implement actual database insert logic
            // var result = await _repository.ExecuteNonQueryAsync(new InsertPaymentMethodCriteria(paymentMethod));

            return new ResultModel("0", "Success");
        }
        catch (Exception ex)
        {
            return new ResultModel("1", ex.Message);
        }
    }
} 