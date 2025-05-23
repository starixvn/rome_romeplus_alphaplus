using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IWithdrawalService
{
    Task<Result> CreateManualWithdrawal(CreateManualWithdrawalRequest request);
    Task<Result> CreateAutoWithdrawal(CreateAutoWithdrawalRequest request);
}

public class WithdrawalService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger,
    IEnumerable<IPaymentProviderService> paymentProviders) : PaymentService(repository, siteDBService, logger), IWithdrawalService
{
    private readonly IEnumerable<IPaymentProviderService> _paymentProviders = paymentProviders;

    public async Task<Result> CreateManualWithdrawal(CreateManualWithdrawalRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var isEnableCashService = await IsEnableCashService(request.AgentId, siteDBConfig.CustDBGroupId);
        if (!isEnableCashService)
        {
            return new Result(
                601,
                "Cash service is not enabled for this agent.");
        }

        var memberSetting = await GetMemberSetting(request.CustId, siteDBConfig.CustDBGroupId);

        // Process withdrawal through payment provider
        var provider = GetPaymentProvider(request.PaymentFrom);
        if (provider == null)
        {
            return new Result(606, "Invalid payment provider");
        }

        var providerResult = await provider.ProcessWithdrawal(
            request.PaymentTo,
            request.Amount,
            "USD", // TODO: Get currency from configuration
            false);

        if (!providerResult.IsSuccessful)
        {
            return providerResult;
        }

        var createManualWithdrawalCriteria = new CreateManualWithdrawalCriteria(
            request.CustId,
            request.AgentId,
            memberSetting.AgentGivenCredit,
            memberSetting.CreditMembersGivenCredit,
            memberSetting.MinLimitAmount,
            memberSetting.MaxLimitAmount,
            request.PaymentFrom,
            request.PaymentTo,
            1,
            request.Amount,
            request.IP,
            siteDBConfig.SiteDBGroupId);

        var logEntries = await repository.FetchAsync<LogEntry>(createManualWithdrawalCriteria);
        var result = new Result(
            createManualWithdrawalCriteria.ErrCode,
            createManualWithdrawalCriteria.ErrMsg);

        if (result.IsSuccessful)
        {
            SyncCashAccumCredit(
                createManualWithdrawalCriteria.CashAccumCreditTbl,
                siteDBConfig.CustDBGroupId);

            await WriteLogs(logEntries, siteDBConfig);
        }

        return result;
    }

    public async Task<Result> CreateAutoWithdrawal(CreateAutoWithdrawalRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var isEnableCashService = await IsEnableCashService(request.AgentId, siteDBConfig.CustDBGroupId);
        if (!isEnableCashService)
        {
            return new Result(
                601,
                "Cash service is not enabled for this agent.");
        }

        var memberSetting = await GetMemberSetting(request.CustId, siteDBConfig.CustDBGroupId);

        // Process withdrawal through payment provider
        var provider = GetPaymentProvider(request.PaymentFrom);
        if (provider == null)
        {
            return new Result(606, "Invalid payment provider");
        }

        var providerResult = await provider.ProcessWithdrawal(
            request.PaymentTo,
            request.Amount,
            "USD", // TODO: Get currency from configuration
            request.IsAutoApprove);

        if (!providerResult.IsSuccessful)
        {
            return providerResult;
        }

        var createAutoWithdrawalCriteria = new CreateAutoWithdrawalCriteria(
            request.CustId,
            request.AgentId,
            memberSetting.AgentGivenCredit,
            memberSetting.CreditMembersGivenCredit,
            memberSetting.MinLimitAmount,
            memberSetting.MaxLimitAmount,
            request.PaymentFrom,
            request.PaymentTo,
            1,
            request.Amount,
            request.IP,
            request.IsAutoApprove,
            siteDBConfig.SiteDBGroupId);

        var logEntries = await repository.FetchAsync<LogEntry>(createAutoWithdrawalCriteria);
        var result = new Result(
            createAutoWithdrawalCriteria.ErrCode,
            createAutoWithdrawalCriteria.ErrMsg);

        if (result.IsSuccessful)
        {
            SyncCashAccumCredit(
                createAutoWithdrawalCriteria.CashAccumCreditTbl,
                siteDBConfig.CustDBGroupId);

            await WriteLogs(logEntries, siteDBConfig);
        }

        return result;
    }

    private IPaymentProviderService GetPaymentProvider(string paymentFrom)
    {
        // TODO: Implement logic to determine provider based on paymentFrom
        // This is a placeholder implementation
        if (paymentFrom.StartsWith("U2"))
        {
            return _paymentProviders.FirstOrDefault(p => p.Provider == PaymentProvider.U2Pay);
        }
        else if (paymentFrom.StartsWith("DIR"))
        {
            return _paymentProviders.FirstOrDefault(p => p.Provider == PaymentProvider.DirePay);
        }
        return null;
    }
} 