using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;
using System.Text;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetProviders(int custId);
    Task<int> GetDailyWithdrawalCount(int custId);
    Task<ResultModel> SetDailyWithdrawalCount(int custId, int count);
    Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int custId);
    Task<ProviderModel> GetProviderById(int custId, int providerId);
    Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods();
    Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int method, int status, int transactionType);
    Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod);
    Task<IEnumerable<BankModel>> GetBanks(int transactionType);
    Task<ResultModel> UpdateProvider(ProviderModel provider);
    
    Task<ResultModel> AddPaymentProviderSetting(ProviderSettingsModel setting);
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

    public async Task<IEnumerable<ProviderModel>> GetProviders(int custId)
    {
        var providers = await _repository.FetchAsync<ProviderModel>(new GetProvidersCriteria(custId));
        
        return providers;
    }

    public async Task<int> GetDailyWithdrawalCount(int custId)
    {
        var result = await _repository.FetchAsync<int>(new GetDailyLimitCriteria(custId, ModeViewConstants.PaymentGateway));

        return result.FirstOrDefault();
    }
    
    public async Task<ResultModel> SetDailyWithdrawalCount(int custId, int count)
    {
        var criteria = new SetDailyWithdrawalCountCriteria(custId, ModeViewConstants.PaymentGateway, count);    
        await _repository.ExecuteAsync(criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }
    
    public async Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int custId)
    {
        var fees = await _repository.FetchAsync<ProviderFeeModel>(new GetProvidersFeesCriteria(custId));
        
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

    public async Task<ProviderModel> GetProviderById(int custId, int providerId)
    {
        var providers = await GetProviders(custId);
        var provider = providers.FirstOrDefault(p => p.Id == providerId);
        return provider;
    }
    
    public async Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods()
    {
        var methods = await _repository.FetchAsync<PaymentMethodModel>(new GetPaymentMethodsCriteria());
        return methods;
    }

    public async Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int method, int status, int transactionType)
    {
        var settings = await _repository.FetchAsync<ProviderSettingsModel>(new GetProviderSettingsCriteria(providerId, method, status, transactionType));
        
        return settings;
    }
    
    public async Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod)
    {
        var criteria = new InsertPaymentMethodCriteria(paymentMethod);
        await _repository.ExecuteAsync(criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }
  
    public async Task<IEnumerable<BankModel>> GetBanks(int transactionType)
    {
        var banks = await _repository.FetchAsync<BankModel>(new GetBanksCriteria(transactionType));
        return banks;
    }

    public async Task<ResultModel> UpdateProvider(ProviderModel provider)
    {
        var updateStatusCriteria = new UpdateProviderStatusCriteria(provider.CustId, provider.Id, provider.Status);

        await _repository.ExecuteAsync(updateStatusCriteria);
        if(updateStatusCriteria.ErrCode != 0)
        {
            return new ResultModel(updateStatusCriteria.ErrCode, updateStatusCriteria.ErrMsg);
        }
        
        if(provider.ProviderSettings != null && provider.ProviderSettings.Count > 0)
        {
            var jsonSetting = new List<string>();
            foreach (var setting in provider.ProviderSettings)
            {   
                jsonSetting.Add($"{{\"MethodID\": {setting.Method}, \"Status\": {setting.Status}, \"MinAmount\": {setting.MinAmount}, \"MaxAmount\": {setting.MaxAmount}, \"Enabled\": {(setting.IsEnabled ? 1 : 0)}}}");
            }

            var jsonSettings = $"[{string.Join(",", jsonSetting)}]";
            var updateProviderSettingsCriteria = new UpdateProviderSettingsCriteria(provider.CustId, provider.Id, jsonSettings);
            await _repository.ExecuteAsync(updateProviderSettingsCriteria);

            if(updateProviderSettingsCriteria.ErrCode != 0)
            {
                return new ResultModel(updateProviderSettingsCriteria.ErrCode, updateProviderSettingsCriteria.ErrMsg);
            }
        }
            
        return new ResultModel(0, string.Empty);
    }


    public async Task<ResultModel> AddPaymentProviderSetting(ProviderSettingsModel setting)
    {
        if (setting == null)
        {
            return new ResultModel(1, "No setting provided");
        }

        // TODO: Implement actual database insert logic
        // var result = await _repository.ExecuteNonQueryAsync(new AddPaymentProviderSettingCriteria(setting));

        return new ResultModel(0, "Success");
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
            return new ResultModel(1, "Invalid account ID");
        }

        // TODO: Implement actual database delete logic
        // var result = await _repository.ExecuteNonQueryAsync(new DeleteBankAccountCriteria(accountId));

        return new ResultModel(0, "Bank account deleted successfully");
    }

    public async Task<ResultModel> SetTransactionLimit(int custId, TransactionLimitModel limit)
    {
        if (custId <= 0)
        {
            return new ResultModel(1, "Invalid customer ID");
        }

        // TODO: Implement actual database update logic
        // var result = await _repository.ExecuteNonQueryAsync(new SetTransactionLimitCriteria(custId, limit, transactionType));

        return new ResultModel(0, "Transaction limit updated successfully");
    }

    public async Task<ResultModel> AddBankAccount(BankAccountModel bankAccount)
    {
        if (bankAccount == null)
        {
            return new ResultModel(1, "No bank account provided");
        }

        // TODO: Implement actual database insert logic
        // var result = await _repository.ExecuteNonQueryAsync(new AddBankAccountCriteria(bankAccount));

        return new ResultModel(0, "Bank account added successfully");
    }

    public async Task<ResultModel> UpdateBankAccount(BankAccountModel bankAccount)
    {
        if (bankAccount == null)
        {
            return new ResultModel(1, "No bank account provided");
        }

        if (bankAccount.Id <= 0)
        {
            return new ResultModel(1, "Invalid bank account ID");
        }

        // TODO: Implement actual database update logic
        // var result = await _repository.ExecuteNonQueryAsync(new UpdateBankAccountCriteria(bankAccount));

        return new ResultModel(0, "Bank account updated successfully");
    }
} 