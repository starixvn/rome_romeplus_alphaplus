using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public abstract class PaymentService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger)
{
    private const byte CashServiceType = 1;

    protected readonly IDynamicRepository repository = repository;
    protected readonly ISiteDBService siteDBService = siteDBService;
    protected readonly Fanex.Logging.ILogger logger = logger;

    public async Task<bool> IsEnableCashService(int agentId, int custDBGroupId)
    {
        var criteria = new GetAgentServiceTypeCriteria(agentId, custDBGroupId);
        await repository.ExecuteAsync(criteria);

        return criteria.ServiceType == CashServiceType;
    }

    public async Task<MemberSetting> GetMemberSetting(int memberId, int custDBGroupId)
    {
        var criteria = new GetMemberSettingCriteria(memberId, custDBGroupId);
        await repository.ExecuteAsync(criteria);

        return new MemberSetting(criteria.AgentGivenCredit, criteria.CreditMembers_GivenCredit, criteria.MinLimitAmt, criteria.MaxLimitAmt);
    }

    public void SyncCashAccumCredit(string cashAccumCreditTbl, int custDbGroupId)
    {
        _ = Task.Run(async () =>
        {
            var criteria = new SyncCashAccumCreditCriteria(cashAccumCreditTbl, custDbGroupId);
            try
            {
                await repository.ExecuteAsync(criteria);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync($"{ex.Message}. {nameof(SyncCashAccumCredit)} failed. Criteria: {JsonHelper.ToJson(criteria)}", ex);
            }
        });
    }

    public async Task WriteLogs(IEnumerable<LogEntry> logEntries, SiteDBConfig siteDBConfig)
    {
        foreach (var item in logEntries)
        {
            if (item.LogTypeId == 1)
            {
                await WriteLogTransfer(item, siteDBConfig.SiteDBGroupId);
            }
            else if (item.LogTypeId is 2 or 3 or 4)
            {
                await WriteLogRunSP(item, siteDBConfig.CustDBGroupId);
            }
        }
    }

    public async Task WriteLogTransfer(LogEntry log, int siteDBGroupId)
    {
        var criteria = new WriteLogTransferCriteria(
            log.UserId,
            log.FromCustId,
            log.ToCustId,
            log.IP,
            log.Doing);
        var functionName = nameof(WriteLogTransfer);
        var isSuccess = await WriteLog(criteria, functionName);

        if (!isSuccess)
        {
            criteria.SiteDBGroupId = siteDBGroupId;
            await WriteLog(criteria, functionName);
        }
    }

    public async Task WriteLogRunSP(LogEntry log, int custDBGroupId)
    {
        var criteria = new WriteLogRunSPCriteria(
            log.UserId,
            log.CustId,
            log.ActionId,
            log.TrackingValue,
            log.TrackingValue2);
        var functionName = nameof(WriteLogRunSP);
        var isSuccess = await WriteLog(criteria, functionName);

        if (!isSuccess)
        {
            criteria.CustDBGroupId = custDBGroupId;
            await WriteLog(criteria, functionName);
        }
    }

    private async Task<bool> WriteLog(INonQueryCommand criteria, string functionName)
    {
        try
        {
            await repository.ExecuteAsync(criteria);
            return true;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"{ex.Message}. {functionName} failed. Criteria {JsonHelper.ToJson(criteria)}", ex);
            return false;
        }
    }
}