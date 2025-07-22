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
    protected readonly IDynamicRepository repository = repository;
    protected readonly ISiteDBService siteDBService = siteDBService;
    protected readonly Fanex.Logging.ILogger logger = logger;

    public async Task<PaymentResult> ValidatePayment(ValidatePaymentCriteria criteria)
    {
        await repository.ExecuteAsync(criteria);

        if (!string.IsNullOrWhiteSpace(criteria.ErrExtraInfo))
        {
            await logger.ErrorAsync($"{nameof(ValidatePayment)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}");
        }

        return new PaymentResult(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task WriteLogsTransferAndRunSP(IEnumerable<LogEntry> logs, SiteDBConfig siteDBConfig)
    {
        if (logs != null)
        {
            foreach (var item in logs)
            {
                if (item.LogTypeId == 1)
                {
                    await WriteLogTransfer(item, siteDBConfig.SiteDBGroupId);
                }
                else
                {
                    await WriteLogRunSP(item, siteDBConfig.CustDBGroupId);
                }
            }
        }
    }

    public async Task WriteLogsRunSP(IEnumerable<LogEntry> logs, short custDBGroupId)
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
        const string functionName = nameof(WriteLogRunSP);
        var isSuccess = await WriteLog(criteria, functionName);

        if (!isSuccess)
        {
            criteria.CustDBGroupId = custDBGroupId;
            await WriteLog(criteria, functionName);
        }
    }

    public async Task WriteLogTransfer(LogEntry log, short siteDBGroupId)
    {
        var criteria = new WriteLogTransferCriteria(
            log.UserId,
            log.FromCustId,
            log.ToCustId,
            log.IP,
            log.Doing);
        const string functionName = nameof(WriteLogTransfer);
        var isSuccess = await WriteLog(criteria, functionName);

        if (!isSuccess)
        {
            criteria.SiteDBGroupId = siteDBGroupId;
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
            await logger.ErrorAsync($"{functionName} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}", ex);
            return false;
        }
    }

    public async Task<Result> SyncCashAccumCredit(string cashAccumCreditTbl, short custDBGroupId)
    {
        var criteria = new SyncCashAccumCreditCriteria(cashAccumCreditTbl, custDBGroupId);
        await repository.ExecuteAsync(criteria);
        var result = new Result(criteria.ErrCode, criteria.ErrMsg);

        if (!result.IsSuccess)
        {
            await logger.ErrorAsync($"{nameof(SyncCashAccumCredit)} failed. Key: {criteria.GetSettingKey()} Criteria: {JsonHelper.ToJson(criteria)}");
        }

        return result;
    }
}