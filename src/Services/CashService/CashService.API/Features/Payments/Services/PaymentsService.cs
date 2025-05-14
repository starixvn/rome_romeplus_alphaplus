using AlphaPlus.CashService.API.Features.Payments.Constants;
using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Models;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Features.Payments.DTOs;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Constants;

namespace AlphaPlus.CashService.API.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetProviders(int custId);
    Task<int> GetDailyWithdrawalCount(int custId);
    Task<ResultModel> SetDailyWithdrawalCount(SetDailyWithdrawalCountRequest request);
    Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int providerId);
    Task<ProviderModel> GetProviderById(int custId, int providerId);
    Task<IEnumerable<PaymentMethodModel>> GetPaymentMethods(int custId, int providerId, int transactionType);
    Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int method, int status, int transactionType);
    Task<ResultModel> InsertPaymentMethod(InsertPaymentMethodRequest request);
    Task<IEnumerable<BankModel>> GetBanks(int transactionType);
    Task<ResultModel> UpdateProvider(UpdateProviderRequest request);
    Task<IEnumerable<BankModel>> GetBanksByCust(int custId);
    Task<IEnumerable<TransactionTypeModel>> GetTransactionTypes();
    Task<IEnumerable<BankAccountModel>> GetBankAccounts(int custId, int bankId, int transactionType, int status, string searchText);
    Task<TransactionLimitModel> GetTransactionLimit(int custId);
    Task<ResultModel> SetTransactionLimit(SetTransactionLimitRequest request);
    Task<ResultModel> AddBankAccount(AddBankAccountRequest request);
    Task<BankAccountModel> GetBankAccountById(int accountId);
    Task<ResultModel> UpdateBankAccount(UpdateBankAccountRequest request);
    Task<ResultModel> DeleteBankAccount(DeleteBankAccountRequest request);
}

public class PaymentsService(
    IDynamicRepository repository,
    ICacheRefService cacheRefService,
    ICommonService commonService
) : IPaymentsService
{
    private readonly ICommonService _commonService = commonService;
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
    
    public async Task<ResultModel> SetDailyWithdrawalCount(SetDailyWithdrawalCountRequest request)
    {
        var criteria = new SetDailyWithdrawalCountCriteria(request);    
        await repository.ExecuteAsync(criteria);
        await _commonService.WriteLogUserRunSP(ActionLogConstants.SetDailyWithdrawalCount, request, criteria);        
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
    
    public async Task<ResultModel> InsertPaymentMethod(InsertPaymentMethodRequest request)
    {
        if (request.ActorId <= 0 || request.ProviderId <= 0)
        {
            return new ResultModel(1, "Invalid customer ID or provider ID");
        }

        var criteria = new InsertPaymentMethodCriteria(request) {
            APIConfig = $"{{\"APIMethodKey\":\"{request.APIMethodKey}\",\"MerchantID\":\"{request.MerchantID}\",\"AppSecret\":\"{request.AppSecret}\"}}",
        };

        await repository.ExecuteAsync(criteria);
        await _commonService.WriteLogUserRunSP(ActionLogConstants.InsertPaymentMethod, request, criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }
  
    public async Task<IEnumerable<BankModel>> GetBanks(int transactionType)
    {
        var banks = await repository.FetchAsync<BankModel>(new GetBanksByTransactionTypeCriteria(transactionType));
        foreach (var bank in banks)
        {
            bank.Name = cacheRefService.GetBankName(bank.Id, bank.Bank);

            if(bank.DepositMode != null)
            {
                bank.DepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.DepositMode);
                bank.DepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.DepositMatchingMode.SourceType, bank.DepositMatchingMode.SourceType);
            }
        }

        return banks;
    }

    public async Task<ResultModel> UpdateProvider(UpdateProviderRequest request)
    {
        var updateStatusCriteria = new UpdateProviderStatusCriteria(request);

        await repository.ExecuteAsync(updateStatusCriteria);
        await _commonService.WriteLogUserRunSP(ActionLogConstants.UpdateProviderStatus, request, updateStatusCriteria);

        if(updateStatusCriteria.ErrCode != 0)
        {
            return new ResultModel(updateStatusCriteria.ErrCode, updateStatusCriteria.ErrMsg);
        }
        
        if(request.ProviderSettings == null || request.ProviderSettings.Count == 0)
        {  
            return new ResultModel(0, string.Empty); 
        }

        var jsonSetting = new List<string>();   
        foreach (var setting in request.ProviderSettings)
        {   
            jsonSetting.Add(setting.ToJsonString());
        }

        var jsonSettings = $"[{string.Join(",", jsonSetting)}]";
        var updateProviderSettingsCriteria = new UpdateProviderSettingsCriteria(request, jsonSettings);
        await repository.ExecuteAsync(updateProviderSettingsCriteria);

        await _commonService.WriteLogUserRunSP(ActionLogConstants.UpdateProviderSettings, request, updateProviderSettingsCriteria);

        return new ResultModel(updateProviderSettingsCriteria.ErrCode, updateProviderSettingsCriteria.ErrMsg);
    }

    public async Task<IEnumerable<BankModel>> GetBanksByCust(int custId)
    {
        var banks = await repository.FetchAsync<BankModel>(new GetBanksByCustCriteria(custId));
        foreach (var bank in banks)
        {
            bank.Name = cacheRefService.GetBankName(bank.Id, bank.Bank);
            if(bank.DepositMode != null)
            {
                bank.DepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.DepositMode);
                bank.DepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.DepositMatchingMode.SourceType, bank.DepositMatchingMode.SourceType);
            }
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
            if(bank.Bank_DepositMatchingMode != null)
            {
                bank.BankDepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.Bank_DepositMatchingMode);
                bank.BankDepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.BankDepositMatchingMode.SourceType, bank.BankDepositMatchingMode.SourceType);
            }
        }

        return accounts;
    }
    
    public async Task<TransactionLimitModel> GetTransactionLimit(int custId)
    {
        var (limits, count) = await repository.FetchMultipleAsync<DailyLimitDTO, Int16>(new GetDailyLimitCriteria(custId, ModeViewConstants.BankAccount));

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
    
    public async Task<ResultModel> SetTransactionLimit(SetTransactionLimitRequest request)
    {
        var criteria = new SetTransactionLimitCriteria(request);
        await repository.ExecuteAsync(criteria);

        await _commonService.WriteLogUserRunSP(ActionLogConstants.SetTransactionLimit, request, criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<ResultModel> AddBankAccount(AddBankAccountRequest request)
    {
        var criteria = new AddBankAccountCriteria(request);
        await repository.ExecuteAsync(criteria);

        await _commonService.WriteLogUserRunSP(ActionLogConstants.AddBankAccount, request, criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<BankAccountModel> GetBankAccountById(int accountId)
    {
        var criteria = new GetBankAccountByIdCriteria(accountId);
        var accounts = await repository.FetchAsync<BankAccountModel>(criteria);
        foreach (var bank in accounts)
        {
            bank.BankName = cacheRefService.GetBankName(bank.Id, bank.BankName);

            if(bank.Bank_DepositMatchingMode != null)
            {
                bank.BankDepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.Bank_DepositMatchingMode);
                bank.BankDepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.BankDepositMatchingMode.SourceType, bank.BankDepositMatchingMode.SourceType);
            }
        }

        return accounts.FirstOrDefault();
    }

    public async Task<ResultModel> UpdateBankAccount(UpdateBankAccountRequest request)
    {
        var criteria = new UpdateBankAccountCriteria(request);
        await repository.ExecuteAsync(criteria);

        await _commonService.WriteLogUserRunSP(ActionLogConstants.UpdateBankAccount, request, criteria);

        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task<ResultModel> DeleteBankAccount(DeleteBankAccountRequest request)
    {
        var criteria = new DeleteBankAccountCriteria(request);
        await repository.ExecuteAsync(criteria);
        await _commonService.WriteLogUserRunSP(ActionLogConstants.DeleteBankAccount, request, criteria);
        return new ResultModel(criteria.ErrCode, criteria.ErrMsg);
    }
} 