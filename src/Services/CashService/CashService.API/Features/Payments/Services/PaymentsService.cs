using AlphaPlus.CashService.API.Features.Payments.Constants;
using AlphaPlus.CashService.API.Features.Payments.Criteria;
using AlphaPlus.CashService.API.Features.Payments.Models;
using AlphaPlus.CashService.API.Features.Payments.Requests;
using Fanex.Data.Repository;
using AlphaPlus.CashService.API.Features.Payments.DTOs;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Payments.ViewModels;
using AlphaPlus.CashService.API.Features.Shared.Services;
using AlphaPlus.CashService.API.Features.Shared.Requests.PaymentGateway;
using AlphaPlus.CashService.API.Features.Shared.Models.PaymentGateway;

namespace AlphaPlus.CashService.API.Features.Payments.Services;

public interface IPaymentsService
{
    Task<IEnumerable<ProviderModel>> GetProviders(int custId, string languageCode);
    Task<int> GetDailyWithdrawalCount(int custId);
    Task<ResultModel> SetDailyWithdrawalCount(SetDailyWithdrawalCountRequest request);
    Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int providerId, string languageCode);
    Task<ProviderModel> GetProviderById(int custId, int providerId, string languageCode);
    Task<AddPaymentMethodViewModel> GetAddPaymentMethodInfo(int custId, int providerId, int transactionType, string languageCode);
    Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int custId, int method, int status, int transactionType, string languageCode);
    Task<ResultModel> InsertPaymentMethod(InsertPaymentMethodRequest request);
    Task<IEnumerable<BankModel>> GetBanks(string site, int transactionType, string languageCode);
    Task<ResultModel> UpdateProvider(UpdateProviderRequest request);
    Task<IEnumerable<BankModel>> GetBanksByCust(int custId, string languageCode);
    Task<IEnumerable<TransactionTypeModel>> GetTransactionTypes();
    Task<IEnumerable<BankAccountModel>> GetBankAccounts(int custId, int bankId, int transactionType, int status, string searchText, string languageCode);
    Task<TransactionLimitModel> GetTransactionLimit(int custId);
    Task<ResultModel> SetTransactionLimit(SetTransactionLimitRequest request);
    Task<ResultModel> AddBankAccount(AddBankAccountRequest request);
    Task<BankAccountModel> GetBankAccountById(int accountId, string languageCode);
    Task<ResultModel> UpdateBankAccount(UpdateBankAccountRequest request);
    Task<ResultModel> DeleteBankAccount(DeleteBankAccountRequest request);

    Task<ResultModel> UpdatePaymentMethod(UpdatePaymentMethodRequest request);
}

public class PaymentsService(
    IDynamicRepository repository,
    ICacheRefService cacheRefService,
    ICommonService commonService,
    IPaymentGatewayService paymentGatewayService
) : IPaymentsService
{
    public async Task<IEnumerable<ProviderModel>> GetProviders(int custId, string languageCode)
    {
        var providers = await repository.FetchAsync<ProviderModel>(new GetProvidersCriteria(custId));

        foreach (var provider in providers)
        {
            provider.Name = cacheRefService.GetProviderName(provider.Id, provider.Name, languageCode);
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
        var result = await commonService.RunDBCommand(request, criteria); 
        return result;
    }
    
    public async Task<IEnumerable<ProviderFeeGroupModel>> GetProviderFees(int providerId, string languageCode)
    {
        var fees = await repository.FetchAsync<ProviderFeeModel>(new GetProvidersFeesCriteria(providerId));
        foreach (var fee in fees)
        {
            fee.Method = cacheRefService.GetPaymentMethodName(fee.MethodId, fee.Method, languageCode);
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

    public async Task<ProviderModel> GetProviderById(int custId, int providerId, string languageCode)
    {
        var providers = await GetProviders(custId, languageCode);
        var provider = providers.FirstOrDefault(p => p.Id == providerId);
        return provider;
    }
    
    public async Task<AddPaymentMethodViewModel> GetAddPaymentMethodInfo(int custId, int providerId, int transactionType, string languageCode)
    {
        var criteria = new GetAddPaymentMethodInfoCriteria(custId, providerId, transactionType);
        var methods = await repository.FetchAsync<PaymentMethodModel>(criteria);
        foreach (var method in methods)
        {
            method.Name = cacheRefService.GetPaymentMethodName(method.Id, method.Name, languageCode);
        }

        return new AddPaymentMethodViewModel {
            PaymentMethods = methods,
            ChannelContact = string.IsNullOrEmpty(criteria.ChannelContact) ? new ChannelContactModel() : Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelContactModel>(criteria.ChannelContact)
        };
    }

    public async Task<IEnumerable<ProviderSettingsModel>> GetProviderSettings(int providerId, int custId, int method, int status, int transactionType, string languageCode)
    {
        var settings = await repository.FetchAsync<ProviderSettingsModel>(new GetProviderSettingsCriteria(providerId, custId, method, status, transactionType));
        foreach (var setting in settings)
        {
            setting.MethodName = cacheRefService.GetPaymentMethodName(setting.Method, setting.MethodName, languageCode);
        }

        return settings;
    }
    
    public async Task<ResultModel> InsertPaymentMethod(InsertPaymentMethodRequest request)
    {
        if (!request.IsValid())
        {
            return new ResultModel(1, "Invalid customer ID or provider ID");
        }

        var apiRequest = new TestConnectionRequest {
            ProviderId = request.ProviderId,
            TransactionType = request.TransactionType,
            PaymentMethod = request.Method,
            APIConfig = new APIConfigModel {
                MerchantNo = request.MerchantID,
                AppSecret = request.AppSecret,
                SecretKey = request.APIMethodKey
            }
        };

        var apiResponse = await paymentGatewayService.TestConnection(apiRequest, request);
        var criteria = new InsertPaymentMethodCriteria(request, apiResponse.Code != 0);
        var result = await commonService.RunDBCommand(request, criteria); 
        return result;
    }
  
    public async Task<IEnumerable<BankModel>> GetBanks(string site, int transactionType, string languageCode)
    {
        var banks = await repository.FetchAsync<BankModel>(new GetBanksByTransactionTypeCriteria(site, transactionType));
        foreach (var bank in banks)
        {
            bank.Name = cacheRefService.GetBankName(bank.Id, bank.Bank, languageCode);

            if(bank.DepositMode != null)
            {
                bank.DepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.DepositMode);
                bank.DepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.DepositMatchingMode.SourceType, bank.DepositMatchingMode.SourceType, languageCode);
            }
        }

        return banks;
    }

    public async Task<ResultModel> UpdateProvider(UpdateProviderRequest request)
    {
        var result = await UpdateProviderStatus(request);

        if(!result.IsSuccessful)
        {
            return result;
        }

        return await UpdateProviderPaymentMethods(request);
    }

    public async Task<ResultModel> UpdateProviderPaymentMethods(UpdateProviderRequest request)
    {
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
        return await commonService.RunDBCommand(request, updateProviderSettingsCriteria);
    }

    public async Task<ResultModel> UpdateProviderStatus(UpdateProviderRequest request)
    {
        if(request.Status < 0) {
            return new ResultModel(0, string.Empty);
        }

        var updateStatusCriteria = new UpdateProviderStatusCriteria(request);
        return await commonService.RunDBCommand(request, updateStatusCriteria);
    }

    public async Task<IEnumerable<BankModel>> GetBanksByCust(int custId, string languageCode)
    {
        var banks = await repository.FetchAsync<BankModel>(new GetBanksByCustCriteria(custId));
        foreach (var bank in banks)
        {
            bank.Name = cacheRefService.GetBankName(bank.Id, bank.Bank, languageCode);
            if(bank.DepositMode != null)
            {
                bank.DepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.DepositMode);
                bank.DepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.DepositMatchingMode.SourceType, bank.DepositMatchingMode.SourceType, languageCode);
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
        string searchText,
        string languageCode
    )
    {
        var criteria = new GetBankAccountsCriteria(custId, bankId, transactionType, status, searchText);
        var accounts = await repository.FetchAsync<BankAccountModel>(criteria);

        foreach (var bank in accounts)
        {
            bank.BankName = cacheRefService.GetBankName(bank.BankId, bank.BankName, languageCode);
            
            if(bank.Bank_DepositMatchingMode != null)
            {
                bank.BankDepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.Bank_DepositMatchingMode);
                bank.BankDepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.BankDepositMatchingMode.SourceType, bank.BankDepositMatchingMode.SourceType, languageCode);
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
        };
    }
    
    public async Task<ResultModel> SetTransactionLimit(SetTransactionLimitRequest request)
    {
        var criteria = new SetTransactionLimitCriteria(request);
        var result = await commonService.RunDBCommand(request, criteria);
        return result;
    }

    public async Task<ResultModel> AddBankAccount(AddBankAccountRequest request)
    {
        var criteria = new AddBankAccountCriteria(request);
        var result = await commonService.RunDBCommand(request, criteria);
        return result;
    }

    public async Task<BankAccountModel> GetBankAccountById(int accountId, string languageCode)
    {
        var criteria = new GetBankAccountByIdCriteria(accountId);
        var accounts = await repository.FetchAsync<BankAccountModel>(criteria);
        foreach (var bank in accounts)
        {
            bank.BankName = cacheRefService.GetBankName(bank.Id, bank.BankName, languageCode);

            if(bank.Bank_DepositMatchingMode != null)
            {
                bank.BankDepositMatchingMode = Newtonsoft.Json.JsonConvert.DeserializeObject<DepositMatchingModeModel>(bank.Bank_DepositMatchingMode);
                bank.BankDepositMatchingMode.SourceTypeName = cacheRefService.GetDepositAutoModeName(bank.BankDepositMatchingMode.SourceType, bank.BankDepositMatchingMode.SourceType, languageCode);
            }
        }

        return accounts.FirstOrDefault();
    }

    public async Task<ResultModel> UpdateBankAccount(UpdateBankAccountRequest request)
    {
        var criteria = new UpdateBankAccountCriteria(request);
        var result = await commonService.RunDBCommand(request, criteria);
        return result;
    }

    public async Task<ResultModel> DeleteBankAccount(DeleteBankAccountRequest request)
    {
        var criteria = new DeleteBankAccountCriteria(request);
        var result = await commonService.RunDBCommand(request, criteria);
        return result;
    }
    
    public async Task<ResultModel> UpdatePaymentMethod(UpdatePaymentMethodRequest request)
    {
        if (!request.IsValid())
        {
            return new ResultModel(1, "Invalid customer ID or provider ID");
        }

        var apiRequest = new TestConnectionRequest {
            ProviderId = request.ProviderId,
            TransactionType = request.TransactionType,
            PaymentMethod = request.Method,
            APIConfig = new APIConfigModel {
                MerchantNo = request.MerchantID,
                AppSecret = request.AppSecret,
                SecretKey = request.APIMethodKey
            }
        };

        var apiResponse = await paymentGatewayService.TestConnection(apiRequest, request);
        var criteria = new UpdatePaymentMethodCriteria(request, apiResponse.Code != 0);
        var result = await commonService.RunDBCommand(request, criteria); 
        return result;
    }
} 