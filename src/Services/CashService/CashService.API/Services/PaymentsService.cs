using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;
using CashService.API.DTOs;
using System.Text.Json;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetProviders(int custId);
    Task<int> GetDailyWithdrawalCount(int custId);
    Task<ResultModel> SetDailyWithdrawalCount(int custId, int count);
    Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int custId);
    Task<ProviderModel> GetProviderById(int custId, int providerId);
    Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods(int providerId, int transactionType);
    Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int method, int status, int transactionType);
    Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod);
    Task<IEnumerable<BankModel>> GetBanks(int transactionType);
    Task<ResultModel> UpdateProvider(ProviderModel provider);
    Task<IEnumerable<BankModel>> GetBanksByCust(int custId);
    Task<IEnumerable<TransactionTypeModel>> GetTransactionTypes();
    Task<IEnumerable<BankAccountModel>> GetBankAccounts(int custId, int bankId, int transactionType, int status, string searchText);
    Task<TransactionLimitModel> GetTransactionLimit(int custId);
    Task<ResultModel> SetTransactionLimit(int custId, TransactionLimitModel limit);
    Task<ResultModel> AddBankAccount(BankAccountModel bankAccount);
    Task<BankAccountModel> GetBankAccountById(int accountId);
    Task<ResultModel> UpdateBankAccount(BankAccountModel bankAccount);
    Task<ResultModel> DeleteBankAccount(int accountId);
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
        var criteria = new SetDailyWithdrawalCountCriteria(custId, count);    
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
    
    public async Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods(int providerId, int transactionType)
    {
        var methods = await _repository.FetchAsync<PaymentMethodModel>(new GetPaymentMethodsCriteria(providerId, transactionType));
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
        var banks = await _repository.FetchAsync<BankModel>(new GetBanksByTransactionTypeCriteria(transactionType));
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

    public async Task<IEnumerable<BankModel>> GetBanksByCust(int custId)
    {
        var banks = await _repository.FetchAsync<BankModel>(new GetBanksByCustCriteria(custId));
        return banks;
    }

    public async Task<IEnumerable<TransactionTypeModel>> GetTransactionTypes()
    {
        var transactionTypes = await _repository.FetchAsync<TransactionTypeModel>(new GetTransactionTypesCriteria());
        return transactionTypes;
    }

    public async Task<IEnumerable<BankAccountModel>> GetBankAccounts(
        int custId, 
        int bankId, 
        int transactionType, 
        int status, 
        string searchText)
    {
        var criteria = new GetBankAccountsCriteria(custId, bankId, transactionType, status, searchText);
        var accounts = await _repository.FetchAsync<BankAccountModel>(criteria);
        return accounts;
    }
    
    public async Task<TransactionLimitModel> GetTransactionLimit(int custId)
    {
        var (limits, count) = await _repository.FetchMultipleAsync<DailyLimitDTO, int>(new GetDailyLimitCriteria(custId, ModeViewConstants.BankAccount));

        var depositLimit = limits.FirstOrDefault(l => l.IsDeposit) ?? new DailyLimitDTO();
        var withdrawalLimit = limits.FirstOrDefault(l => !l.IsDeposit) ?? new DailyLimitDTO();

        return new TransactionLimitModel {
            DailyWithdrawalCount = count.FirstOrDefault(),
            MinPerDepositTransaction = depositLimit.MinPer,
            MaxPerDepositTransaction = depositLimit.MaxPer,
            MinPerWithdrawalTransaction = withdrawalLimit.MinPer,
            MaxPerWithdrawalTransaction = withdrawalLimit.MaxPer,
        } ;
    }
    
    public async Task<ResultModel> SetTransactionLimit(int custId, TransactionLimitModel limit)
    {
        var criteria = new SetTransactionLimitCriteria(
            custId, 
            limit.DailyWithdrawalCount, 
            limit.MinPerDepositTransaction, 
            limit.MaxPerDepositTransaction, 
            limit.MinPerWithdrawalTransaction, 
            limit.MaxPerWithdrawalTransaction);

        await _repository.ExecuteAsync(criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<ResultModel> AddBankAccount(BankAccountModel bankAccount)
    {
        var criteria = new AddBankAccountCriteria(bankAccount);
        await _repository.ExecuteAsync(criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<BankAccountModel> GetBankAccountById(int accountId)
    {
        var criteria = new GetBankAccountByIdCriteria(accountId);
        var accounts = await _repository.FetchAsync<BankAccountModel>(criteria);
        return accounts.FirstOrDefault();
    }

    public async Task<ResultModel> UpdateBankAccount(BankAccountModel bankAccount)
    {
        var criteria = new UpdateBankAccountCriteria(bankAccount);
        await _repository.ExecuteAsync(criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<ResultModel> DeleteBankAccount(int accountId)
    {
        if (accountId <= 0)
        {
            return new ResultModel(1, "Invalid account ID");
        }

        // TODO: Implement actual database delete logic
        // var result = await _repository.ExecuteNonQueryAsync(new DeleteBankAccountCriteria(accountId));

        return new ResultModel(0, string.Empty);
    }
} 