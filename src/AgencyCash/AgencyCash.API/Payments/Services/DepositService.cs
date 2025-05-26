using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IDepositService
{
    Task<Result> Manual(ManualDepositRequest request);
}

public class DepositService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : PaymentService(repository, siteDBService, logger), IDepositService
{
    public async Task<Result> Manual(ManualDepositRequest request)
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

        var criteria = new ManualDepositCriteria(
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
            request.PaySlip,
            request.IP,
            siteDBConfig.SiteDBGroupId);
        var logEntries = await repository.FetchAsync<LogEntry>(criteria);
        var result = new Result(
            criteria.ErrCode,
            criteria.ErrMsg).Data = criteria.TransId;

        // if (result.IsSuccessful)
        // {
        SyncCashAccumCredit(criteria.CashAccumCreditTbl, siteDBConfig.CustDBGroupId);

        await WriteLogs(logEntries, siteDBConfig);
        //}

        return null;
    }
}