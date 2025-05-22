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
}

public class WithdrawalService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : PaymentService(repository, siteDBService, logger), IWithdrawalService
{
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

        if (result.IsSuccess)
        {
            SyncCashAccumCredit(
                createManualWithdrawalCriteria.CashAccumCreditTbl,
                siteDBConfig.CustDBGroupId);

            await WriteLogs(logEntries, siteDBConfig);
        }

        return result;
    }
} 