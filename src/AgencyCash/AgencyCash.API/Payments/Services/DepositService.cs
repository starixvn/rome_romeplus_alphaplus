using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Services;
using AgencyCash.API.Payments.Constants;
using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IDepositService
{
    Task<DepositLimit> GetLimit(GetDepositLimitRequest request);

    Task<PaymentResult> Manual(ManualDepositRequest request);

    Task<PaymentResult> Auto(AutoDepositRequest request);

    Task<Result> UpdateTransactionStatus(long transId, UpdateTransactionStatusRequest request);

    Task<Result> UpdatePaySlip(long transId, UpdatePaySlipRequest request);
}

public class DepositService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger,
    INotificationsService notificationsService) : PaymentService(repository, siteDBService, logger), IDepositService
{
    private readonly INotificationsService notificationsService = notificationsService;

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
        var validateResult = await ValidatePayment(new ValidatePaymentCriteria(
            request.AgentId,
            request.CustId,
            TransactionType.Deposit,
            siteDBConfig.CustDBGroupId,
            paymentFrom: request.PaymentFrom,
            isManual: true,
            paymentMethodId: request.PaymentMethodId,
            paymentTo: request.PaymentTo));

        if (!validateResult.IsSuccess)
        {
            return validateResult;
        }

        const byte BankAccount = 2;
        var limitCriteria = new GetDepositLimitCriteria(
            request.AgentId,
            request.CustId,
            paymentType: BankAccount,
            custDBGroupId: siteDBConfig.CustDBGroupId);
        await repository.ExecuteAsync(limitCriteria);

        var criteria = new AddTransactionDepositCriteria(
            request.AgentId,
            request.CustId,
            siteDBConfig.SiteDBGroupId,
            limitCriteria.AgentGivenCredit,
            limitCriteria.CreditMembers_GivenCredit,
            limitCriteria.MinAmt,
            limitCriteria.MaxAmt,
            transactionTypeId: TransactionType.Deposit,
            paymentType: BankAccount,
            paymentMethodId: request.PaymentMethodId,
            request.PaymentFrom,
            request.PaymentTo,
            request.Amount,
            ip: request.IP,
            remark: request.Remark);

        var result = await AddTransactionAndWriteLogs(criteria, siteDBConfig.CustDBGroupId);

        if (result.IsSuccess)
        {
            _ = Task.Run(() => NotifyPendingDeposit(request.AgentId, request.CustId));
        }

        return result;
    }

    public async Task<PaymentResult> Auto(AutoDepositRequest request)
    {
        const byte AutoDeposit = 2;

        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);
        var validateResult = await ValidatePayment(new ValidatePaymentCriteria(
            request.AgentId,
            request.CustId,
            TransactionType.Deposit,
            siteDBConfig.CustDBGroupId,
            paymentFrom: request.PaymentFrom,
            providerId: request.ProviderId ?? 0,
            paymentMethodId: request.PaymentMethodId ?? 0,
            paymentTo: request.PaymentTo));

        if (!validateResult.IsSuccess)
        {
            return validateResult;
        }

        var limitCriteria = new GetDepositLimitCriteria(
            request.AgentId,
            request.CustId,
            paymentType: request.DepositMethod,
            providerId: request.ProviderId ?? 0,
            paymentMethodId: request.PaymentMethodId ?? 0,
            custDBGroupId: siteDBConfig.CustDBGroupId);
        await repository.ExecuteAsync(limitCriteria);

        var criteria = new AddTransactionDepositCriteria(
            request.AgentId,
            request.CustId,
            siteDBConfig.SiteDBGroupId,
            limitCriteria.AgentGivenCredit,
            limitCriteria.CreditMembers_GivenCredit,
            limitCriteria.MinAmt,
            limitCriteria.MaxAmt,
            transactionTypeId: AutoDeposit,
            paymentType: request.DepositMethod,
            paymentMethodId: request.PaymentMethodId ?? 0,
            request.PaymentFrom,
            request.PaymentTo,
            request.Amount,
            userInputAmount: request.UserInputAmount,
            matchingMode: request.MatchingMode,
            providerId: request.ProviderId,
            ip: request.IP,
            remark: request.Remark);

        return await AddTransactionAndWriteLogs(criteria, siteDBConfig.CustDBGroupId);
    }

    private async Task<PaymentResult> AddTransactionAndWriteLogs(AddTransactionDepositCriteria criteria, short custDBGroupId)
    {
        var tasks = new List<Task>();

        var logs = await repository.FetchAsync<LogEntry>(criteria);
        var result = new PaymentResult(criteria.ErrCode, criteria.ErrMsg, criteria.TransId);

        if (criteria.PaymentType == AutoDepositType.Gateway
            && result.IsSuccess
            && !string.IsNullOrWhiteSpace(criteria.CashAccumCreditTbl))
        {
            tasks.Add(SyncCashAccumCredit(criteria.CashAccumCreditTbl, custDBGroupId));
        }

        tasks.Add(WriteLogsRunSP(logs, custDBGroupId));

        if (!string.IsNullOrWhiteSpace(criteria.ErrExtraInfo))
        {
            tasks.Add(logger.ErrorAsync($"Deposit failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}"));
        }

        await Task.WhenAll(tasks);

        return result;
    }

    public async Task<Result> UpdateTransactionStatus(long transId, UpdateTransactionStatusRequest request)
    {
        var tasks = new List<Task>();
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var limitCriteria = new GetDepositLimitCriteria(request.AgentId, request.CustId, siteDBConfig.CustDBGroupId);
        await repository.ExecuteAsync(limitCriteria);

        var criteria = new UpdateDepositTransStatusCriteria(
            request.AgentId,
            request.CustId,
            transId,
            request.StatusId,
            siteDBConfig.SiteDBGroupId,
            limitCriteria.AgentGivenCredit,
            limitCriteria.CreditMembers_GivenCredit,
            request.ProviderTransId,
            request.IP);
        var logs = await repository.FetchAsync<LogEntry>(criteria);
        var result = new Result(criteria.ErrCode, criteria.ErrMsg);

        if (result.IsSuccess && !string.IsNullOrWhiteSpace(criteria.CashAccumCreditTbl))
        {
            tasks.Add(SyncCashAccumCredit(criteria.CashAccumCreditTbl, siteDBConfig.CustDBGroupId));
        }

        tasks.Add(WriteLogsTransferAndRunSP(logs, siteDBConfig));

        if (!string.IsNullOrWhiteSpace(criteria.ErrExtraInfo))
        {
            tasks.Add(logger.ErrorAsync($"Deposit - {nameof(UpdateTransactionStatus)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}"));
        }

        await Task.WhenAll(tasks);

        return result;
    }

    public async Task<Result> UpdatePaySlip(long transId, UpdatePaySlipRequest request)
    {
        var tasks = new List<Task>();
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var criteria = new UpdatePaySlipCriteria(
            request.AgentId,
            request.CustId,
            transId,
            request.PaySlipBuffer,
            request.IP,
            siteDBConfig.SiteDBGroupId);
        var logs = await repository.FetchAsync<LogEntry>(criteria);
        tasks.Add(WriteLogsRunSP(logs, siteDBConfig.CustDBGroupId));

        if (!string.IsNullOrWhiteSpace(criteria.ErrExtraInfo))
        {
            tasks.Add(logger.ErrorAsync($"Deposit - {nameof(UpdatePaySlipCriteria)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}"));
        }

        await Task.WhenAll(tasks);

        return new Result(criteria.ErrCode, criteria.ErrMsg);
    }

    public async Task NotifyPendingDeposit(int agentId, int custId)
    {
        try
        {
            await notificationsService.NotifyPendingDeposit(agentId, custId);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"{nameof(NotifyPendingDeposit)} failed. Agent: ${agentId}, CustId: ${custId}", ex);
        }
    }
}