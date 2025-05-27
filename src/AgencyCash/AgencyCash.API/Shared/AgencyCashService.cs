using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Shared;

public abstract class AgencyCashService
{
    protected readonly IDynamicRepository Repository;
    protected readonly ISiteDBService SiteDBService;
    protected readonly Fanex.Logging.ILogger Logger;

    protected AgencyCashService(
        IDynamicRepository repository,
        ISiteDBService siteDBService,
        Fanex.Logging.ILogger logger)
    {
        Repository = repository;
        SiteDBService = siteDBService;
        Logger = logger;
    }

    protected async Task<bool> IsEnableCashService(int agentId, int custDBGroupId)
    {
        // TODO: Implement check if cash service is enabled for agent
        return true;
    }

    protected async Task<MemberSetting> GetMemberSetting(int custId, int custDBGroupId)
    {
        // TODO: Implement getting member settings
        return new MemberSetting(0, 0, 0, 1000000);
    }

    protected void SyncCashAccumCredit(string cashAccumCreditTbl, string custDBGroupId)
    {
        // TODO: Implement syncing cash accum credit
    }

    protected async Task WriteLogs(IEnumerable<LogEntry> logEntries, SiteDBConfig siteDBConfig)
    {
        // TODO: Implement writing logs
    }
} 