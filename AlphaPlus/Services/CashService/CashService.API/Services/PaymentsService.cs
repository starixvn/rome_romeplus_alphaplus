using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetAllPaymentProviders(int custId);

    Task<ProviderModel> GetPaymentProviderById(int providerId);
    Task<IEnumerable<ProviderFeeGroupModel>> GetPaymentProviderFees(int providerId);
    Task<ResultModel> SetDailyWithdrawalCount(int custId, int count);
    Task<IEnumerable<ProviderSettingsModel>> GetPaymentProviderSettings(int providerId, int method, int status, int transactionType);
    Task<ResultModel> UpdatePaymentProviderSettings(IEnumerable<ProviderSettingsModel> settings);
    Task<ResultModel> AddPaymentProviderSetting(ProviderSettingsModel setting);
    Task<IEnumerable<PaymentMethodModel>> GetAllPaymentMethods();
    Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod);
    Task<IEnumerable<BankModel>> GetAllBanks();
    Task<IEnumerable<BankAccountModel>> GetBankAccounts(int custId, int bankId, int transactionType, int status, string searchText);
    Task<ResultModel> DeleteBankAccount(int accountId);
    Task<ResultModel> SetTransactionLimit(int custId, TransactionLimitModel limit);
    Task<ResultModel> AddBankAccount(BankAccountModel bankAccount);
    Task<ResultModel> UpdateBankAccount(BankAccountModel bankAccount);
}

public class PaymentsService : IPaymentsService
{
    private readonly IDynamicRepository _repository;

    public PaymentsService(IDynamicRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProviderModel>> GetAllPaymentProviders(int custId)
    {
        var providers = await _repository.FetchAsync<ProviderModel>(new GetPaymentProvidersCriteria(custId));
        
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
        // TODO: Implement actual database update logic
        // var result = await _repository.ExecuteNonQueryAsync(new SetDailyWithdrawalCountCriteria(custId, count));
        return new ResultModel("0", "Success");
    }

    public async Task<ResultModel> UpdatePaymentProviderSettings(IEnumerable<ProviderSettingsModel> settings)
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

    public async Task<ResultModel> AddPaymentProviderSetting(ProviderSettingsModel setting)
    {
        if (setting == null)
        {
            return new ResultModel("1", "No setting provided");
        }

        // TODO: Implement actual database insert logic
        // var result = await _repository.ExecuteNonQueryAsync(new AddPaymentProviderSettingCriteria(setting));

        return new ResultModel("0", "Success");
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
        if (paymentMethod == null)
        {
            return new ResultModel("1", "No payment method provided");
        }

        // TODO: Implement actual database insert logic
        // var result = await _repository.ExecuteNonQueryAsync(new InsertPaymentMethodCriteria(paymentMethod));

        return new ResultModel("0", "Success");
    }

    public async Task<IEnumerable<BankModel>> GetAllBanks()
    {
        // var result = await _repository.FetchAsync<BankModel>(new GetAllBanksCriteria());
        var banks = await DummyDataService.GetDummyBanks();
        
        return banks;
    }

    public async Task<IEnumerable<BankAccountModel>> GetBankAccounts(
        int custId, 
        int bankId, 
        int transactionType, 
        int status, 
        string searchText)
    {
        // TODO: Implement actual database query
        // var result = await _repository.FetchAsync<BankAccountModel>(new GetBankAccountsCriteria(custId, bankId, transactionType, status, searchText));
        var accounts = await DummyDataService.GetDummyBankAccounts(custId, transactionType);
        
        return accounts;
    }
    
    public async Task<ResultModel> DeleteBankAccount(int accountId)
    {
        if (accountId <= 0)
        {
            return new ResultModel("1", "Invalid account ID");
        }

        // TODO: Implement actual database delete logic
        // var result = await _repository.ExecuteNonQueryAsync(new DeleteBankAccountCriteria(accountId));

        return new ResultModel("0", "Bank account deleted successfully");
    }

    public async Task<ResultModel> SetTransactionLimit(int custId, TransactionLimitModel limit)
    {
        if (custId <= 0)
        {
            return new ResultModel("1", "Invalid customer ID");
        }

        // TODO: Implement actual database update logic
        // var result = await _repository.ExecuteNonQueryAsync(new SetTransactionLimitCriteria(custId, limit, transactionType));

        return new ResultModel("0", "Transaction limit updated successfully");
    }

    public async Task<ResultModel> AddBankAccount(BankAccountModel bankAccount)
    {
        if (bankAccount == null)
        {
            return new ResultModel("1", "No bank account provided");
        }

        // TODO: Implement actual database insert logic
        // var result = await _repository.ExecuteNonQueryAsync(new AddBankAccountCriteria(bankAccount));

        return new ResultModel("0", "Bank account added successfully");
    }

    public async Task<ResultModel> UpdateBankAccount(BankAccountModel bankAccount)
    {
        if (bankAccount == null)
        {
            return new ResultModel("1", "No bank account provided");
        }

        if (bankAccount.Id <= 0)
        {
            return new ResultModel("1", "Invalid bank account ID");
        }

        // TODO: Implement actual database update logic
        // var result = await _repository.ExecuteNonQueryAsync(new UpdateBankAccountCriteria(bankAccount));

        return new ResultModel("0", "Bank account updated successfully");
    }
} 