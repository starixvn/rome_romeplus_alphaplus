using AgencyCash.API.Payments.Criteria.Deposit;
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
    private const int UnableDeposit = 403;

    public async Task<DepositLimit> GetLimit(GetDepositLimitRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var accumCreditCriteria = new GetAgentCashAccumCreditCriteria(request.AgentId, siteDBConfig.SiteDBGroupId);
        await repository.ExecuteAsync(accumCreditCriteria);

        var limitCriteria = new GetDepositLimitCriteria(
            request.CustId,
            request.PaymentType,
            request.ProviderId ?? 0,
            request.PaymentMethodId ?? 0,
            accumCreditCriteria.AccumCreditReturn,
            siteDBConfig.CustDBGroupId);
        await repository.ExecuteAsync(limitCriteria);

        return new DepositLimit(limitCriteria.MinAmt, limitCriteria.MaxAmt, limitCriteria.AgentAvailableCredit);
    }

    public async Task<PaymentResult> Manual(ManualDepositRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        if (!await IsEnableCashService(request.AgentId, siteDBConfig.CustDBGroupId) ||
            !await IsAllowDeposit(request.AgentId, siteDBConfig.CustDBGroupId))
        {
            return new PaymentResult(UnableDeposit, "Unable to process deposits.", 0);
        }

        var criteria = new ManualDepositCriteria(
            request.AgentId,
            request.CustId,
            siteDBConfig.CustDBGroupId,
            0, 0, 0, 0,
            // memberSetting.AgentGivenCredit,
            // memberSetting.CreditMembersGivenCredit,
            // memberSetting.MinLimitAmount,
            // memberSetting.MaxLimitAmount,
            request.PaymentMethodId,
            request.PaymentFrom,
            request.PaymentTo,
            1,
            request.Amount,
            request.PaySlipBuffer,
            request.IP,
            request.Remark);
        var logs = await repository.FetchAsync<LogEntry>(criteria);
        var result = new PaymentResult(criteria.ErrCode, criteria.ErrMsg, criteria.TransId);

        if (result.IsSuccess)
        {
            await WriteLogs(logs, siteDBConfig.CustDBGroupId);
        }

        return result;
    }

    private async Task<bool> IsAllowDeposit(int agentId, short custDBGroupId)
    {
        var criteria = new CheckAllowDepositCriteria(agentId, custDBGroupId);
        await repository.ExecuteAsync(criteria);

        return criteria.IsAllowDeposit;
    }
}