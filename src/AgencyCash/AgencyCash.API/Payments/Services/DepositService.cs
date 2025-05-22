using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IDepositService
{
    Task<IEnumerable<DepositMethod>> GetDepositMethods(int agentId);

    Task<MemberSetting> GetMemberSetting(int memberId, int custDBGroupId);

    Task<Result> CreateManualDeposit(CreateManualDepositRequest request);
}

public class DepositService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : PaymentService(repository, siteDBService, logger), IDepositService
{
    public Task<IEnumerable<DepositMethod>> GetDepositMethods(int agentId)
        => repository.FetchAsync<DepositMethod>(new GetDepositMethodsCriteria(agentId));

    public async Task<MemberSetting> GetMemberSetting(int memberId, int custDBGroupId)
    {
        var criteria = new GetMemberSettingCriteria(memberId, custDBGroupId);
        await repository.ExecuteAsync(criteria);

        return new MemberSetting(criteria.AgentGivenCredit, criteria.CreditMembers_GivenCredit, criteria.MinLimitAmt, criteria.MaxLimitAmt);
    }

    public async Task<Result> CreateManualDeposit(CreateManualDepositRequest request)
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

        var createManualDepositCriteria = new CreateManualDepositCriteria(
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
        var logEntries = await repository.FetchAsync<LogEntry>(createManualDepositCriteria);
        var result = new Result(
            createManualDepositCriteria.ErrCode,
            createManualDepositCriteria.ErrMsg);

        if (result.IsSuccess)
        {
            SyncCashAccumCredit(
                createManualDepositCriteria.CashAccumCreditTbl,
                siteDBConfig.CustDBGroupId);

            await WriteLogs(logEntries, siteDBConfig);
        }

        return result;
    }
}