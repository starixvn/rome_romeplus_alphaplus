using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface ITransactionService
{
    Task<Result> UpdateTransProviderInfo(long transId, UpdateTransProviderInfoRequest request);
}

public class TransactionService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : PaymentService(repository, siteDBService, logger), ITransactionService
{
    public async Task<Result> UpdateTransProviderInfo(long transId, UpdateTransProviderInfoRequest request)
    {
        var tasks = new List<Task>();
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var criteria = new UpdateTransProviderInfoCriteria(
            request.AgentId,
            request.CustId,
            transId,
            request.TransType,
            request.ProviderTransId,
            request.IP,
            siteDBConfig.SiteDBGroupId);
        var logs = await repository.FetchAsync<LogEntry>(criteria);
        var result = new Result(criteria.ErrCode, criteria.ErrMsg);

        tasks.Add(WriteLogsRunSP(logs, siteDBConfig.CustDBGroupId));

        if (!string.IsNullOrWhiteSpace(criteria.ErrExtraInfo))
        {
            tasks.Add(logger.ErrorAsync($"Transaction - {nameof(UpdateTransProviderInfo)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}"));
        }

        await Task.WhenAll(tasks);

        return result;
    }
}