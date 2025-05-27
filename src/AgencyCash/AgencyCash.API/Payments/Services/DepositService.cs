using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IDepositService
{
    Task<DepositLimit> GetLimit(GetDepositLimitRequest request);

    Task<PaymentResult> Manual(ManualDepositRequest request);
}

public class DepositService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : PaymentService(repository, siteDBService, logger), IDepositService
{
    public async Task<DepositLimit> GetLimit(GetDepositLimitRequest request)
    {
        var criteria = new GetDepositLimitCriteria(
            request.AgentId,
            request.CustId,
            request.PaymentType,
            request.ProviderId,
            request.PaymentMethodId);
        await repository.ExecuteAsync(criteria);

        return new DepositLimit(criteria.MinAmt, criteria.MaxAmt);
    }

    public async Task<PaymentResult> Manual(ManualDepositRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var isEnableCashService = await IsEnableCashService(request.AgentId, siteDBConfig.CustDBGroupId);
        if (!isEnableCashService)
        {
            return new PaymentResult(
                601,
                "Cash service is not enabled for this agent.",
                0);
        }

        var memberSetting = await GetMemberSetting(request.CustId, siteDBConfig.CustDBGroupId);

        var criteria = new ManualDepositCriteria(
            request.AgentId,
            request.CustId,
            memberSetting.AgentGivenCredit,
            memberSetting.CreditMembersGivenCredit,
            memberSetting.MinLimitAmount,
            memberSetting.MaxLimitAmount,
            request.PaymentFrom,
            request.PaymentTo,
            1,
            request.Amount,
            request.PaySlipBuffer,
            request.IP,
            siteDBConfig.SiteDBGroupId);
        var logEntries = await repository.FetchAsync<LogEntry>(criteria);
        var result = new PaymentResult(
            criteria.ErrCode,
            criteria.ErrMsg,
            criteria.TransId);

        if (result.IsSuccess)
        {
            SyncCashAccumCredit(criteria.CashAccumCreditTbl, siteDBConfig.CustDBGroupId);

            await WriteLogs(logEntries, siteDBConfig);
        }

        return null;
    }
}