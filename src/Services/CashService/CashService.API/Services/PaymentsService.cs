using AlphaPlus.CashService.API.Constants;
using AlphaPlus.CashService.API.Criteria.Payments;
using AlphaPlus.CashService.API.Models.Payments;
using AlphaPlus.CashService.API.Models;
using Fanex.Data.Repository;
using CashService.API.DTOs.Payments;
using System.Text.Json;
using Fanex.Cryptography.Aes;
using Microsoft.Extensions.Configuration;
using AlphaPlus.Shared;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetProviders(int custId);
    Task<int> GetDailyWithdrawalCount(int custId);
    Task<ResultModel> SetDailyWithdrawalCount(int custId, int count);
    Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int providerId);
    Task<ProviderModel> GetProviderById(int custId, int providerId);
    Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods(int custId, int providerId, int transactionType);
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

public class PaymentsService(
    IDynamicRepository repository,
    ICacheRefService cacheRefService,
    IAesService encryption,
    IConfiguration configuration)
    : IPaymentsService
{
    private readonly string _encryptSecretKey = configuration.GetValue<string>("EncryptSecretKey") ?? AlphaPlusConstants.EncryptSecretKey;

    public async Task<IEnumerable<ProviderModel>> GetProviders(int custId)
    {
        var providers = await repository.FetchAsync<ProviderModel>(new GetProvidersCriteria(custId));

        foreach (var provider in providers)
        {
            provider.Name = cacheRefService.GetProviderName(provider.Id, provider.Name);
        }
        
        return providers;
    }

    public async Task<int> GetDailyWithdrawalCount(int custId)
    {
        var result = await repository.FetchAsync<int>(new GetDailyLimitCriteria(custId, ModeViewConstants.PaymentGateway));

        return result.FirstOrDefault();
    }
    
    public async Task<ResultModel> SetDailyWithdrawalCount(int custId, int count)
    {
        var criteria = new SetDailyWithdrawalCountCriteria(custId, count);    
        await repository.ExecuteAsync(criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }
    
    public async Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int providerId)
    {
        var fees = await repository.FetchAsync<ProviderFeeModel>(new GetProvidersFeesCriteria(providerId));
        foreach (var fee in fees)
        {
            fee.Method = cacheRefService.GetPaymentMethodName(fee.MethodId, fee.Method);
        }

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
    
    public async Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods(int custId, int providerId, int transactionType)
    {
        var methods = await repository.FetchAsync<PaymentMethodModel>(new GetPaymentMethodsCriteria(custId, providerId, transactionType));
        foreach (var method in methods)
        {
            method.Name = cacheRefService.GetPaymentMethodName(method.Id, method.Name);
        }

        return methods;
    }

    public async Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int method, int status, int transactionType)
    {
        var settings = await repository.FetchAsync<ProviderSettingsModel>(new GetProviderSettingsCriteria(providerId, method, status, transactionType));
        foreach (var setting in settings)
        {
            setting.MethodName = cacheRefService.GetPaymentMethodName(setting.Method, setting.MethodName);
        }

        return settings;
    }
    
    public async Task<ResultModel> InsertPaymentMethod(PaymentMethodModel paymentMethod)
    {
        var criteria = new InsertPaymentMethodCriteria(paymentMethod) {
            APIMethodKey = encryption.Encrypt(paymentMethod.APIMethodKey, _encryptSecretKey),
            MerchantID = encryption.Encrypt(paymentMethod.MerchantID, _encryptSecretKey),
            AppSecret = encryption.Encrypt(paymentMethod.AppSecret, _encryptSecretKey)
        };
        
        await repository.ExecuteAsync(criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }
  
    public async Task<IEnumerable<BankModel>> GetBanks(int transactionType)
    {
        var banks = await repository.FetchAsync<BankModel>(new GetBanksByTransactionTypeCriteria(transactionType));
        foreach (var bank in banks)
        {
            bank.Name = cacheRefService.GetBankName(bank.Id, bank.Bank);
        }
        
        return banks;
    }

    public async Task<ResultModel> UpdateProvider(ProviderModel provider)
    {
        var updateStatusCriteria = new UpdateProviderStatusCriteria(provider.CustId, provider.Id, provider.Status);

        await repository.ExecuteAsync(updateStatusCriteria);
        if(updateStatusCriteria.ErrCode != 0)
        {
            return new ResultModel(updateStatusCriteria.ErrCode, updateStatusCriteria.ErrMsg);
        }
        
        if(provider.ProviderSettings != null && provider.ProviderSettings.Count > 0)
        {
            var jsonSetting = new List<string>();
            foreach (var setting in provider.ProviderSettings)
            {   
                jsonSetting.Add(setting.ToJsonString());
            }

            var jsonSettings = $"[{string.Join(",", jsonSetting)}]";
            var updateProviderSettingsCriteria = new UpdateProviderSettingsCriteria(provider.CustId, provider.Id, jsonSettings);
            await repository.ExecuteAsync(updateProviderSettingsCriteria);

            if(updateProviderSettingsCriteria.ErrCode != 0)
            {
                return new ResultModel(updateProviderSettingsCriteria.ErrCode, updateProviderSettingsCriteria.ErrMsg);
            }
        }
            
        return new ResultModel(0, string.Empty);
    }

    public async Task<IEnumerable<BankModel>> GetBanksByCust(int custId)
    {
        var banks = await repository.FetchAsync<BankModel>(new GetBanksByCustCriteria(custId));
        foreach (var bank in banks)
        {
            bank.Name = cacheRefService.GetBankName(bank.Id, bank.Bank);
        }

        return banks;
    }

    public async Task<IEnumerable<TransactionTypeModel>> GetTransactionTypes()
    {
        var transactionTypes = await repository.FetchAsync<TransactionTypeModel>(new GetTransactionTypesCriteria());
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
        var accounts = await repository.FetchAsync<BankAccountModel>(criteria);

        foreach (var bank in accounts)
        {
            bank.AccountHolderName = encryption.Decrypt(bank.AccountHolderName, _encryptSecretKey);
            bank.AccountNumber = encryption.Decrypt(bank.AccountNumber, _encryptSecretKey);
        }

        return accounts;
    }
    
    public async Task<TransactionLimitModel> GetTransactionLimit(int custId)
    {
        var (limits, count) = await repository.FetchMultipleAsync<DailyLimitDTO, int>(new GetDailyLimitCriteria(custId, ModeViewConstants.BankAccount));

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

        await repository.ExecuteAsync(criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<ResultModel> AddBankAccount(BankAccountModel bankAccount)
    {
        var criteria = new AddBankAccountCriteria(bankAccount) {
            AccountHolderName = encryption.Encrypt(bankAccount.AccountHolderName, _encryptSecretKey),
            AccountNumber = encryption.Encrypt(bankAccount.AccountNumber, _encryptSecretKey),
        };
        
        await repository.ExecuteAsync(criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<BankAccountModel> GetBankAccountById(int accountId)
    {
        var criteria = new GetBankAccountByIdCriteria(accountId);
        var accounts = await repository.FetchAsync<BankAccountModel>(criteria);
        foreach (var bank in accounts)
        {
            bank.BankName = cacheRefService.GetBankName(bank.Id, bank.BankName);
            bank.AccountHolderName = encryption.Decrypt(bank.AccountHolderName, _encryptSecretKey);
            bank.AccountNumber = encryption.Decrypt(bank.AccountNumber, _encryptSecretKey);
        }

        return accounts.FirstOrDefault();
    }

    public async Task<ResultModel> UpdateBankAccount(BankAccountModel bankAccount)
    {
        Console.WriteLine(JsonSerializer.Serialize(bankAccount));
        var criteria = new UpdateBankAccountCriteria(bankAccount);
        await repository.ExecuteAsync(criteria);
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