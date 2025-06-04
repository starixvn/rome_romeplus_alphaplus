using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public abstract class PaymentService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger)
{
    protected readonly IDynamicRepository repository = repository;
    protected readonly ISiteDBService siteDBService = siteDBService;
    protected readonly Fanex.Logging.ILogger logger = logger;

    public async Task<Result> ValidatePayment(int agentId, int custId, byte transType, short custDBGroupId)
    {
        var criteria = new ValidatePaymentCriteria(agentId, custId, transType, custDBGroupId);
        await repository.ExecuteAsync(criteria);

        return new Result(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task WriteLogs(IEnumerable<LogEntry> logs, short custDBGroupId)
    {
        if (logs != null)
        {
            foreach (var item in logs)
            {
                await WriteLogRunSP(item, custDBGroupId);
            }
        }
    }

    public async Task WriteLogRunSP(LogEntry log, short custDBGroupId)
    {
        var criteria = new WriteLogRunSPCriteria(
            log.UserId,
            log.CustId,
            log.ActionId,
            log.TrackingValueMask,
            log.TrackingValueMask2,
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
            await logger.ErrorAsync($"{ex.Message}. {functionName} failed. Criteria: {JsonHelper.ToJson(criteria)}", ex);
            return false;
        }
    }
}