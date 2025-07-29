using AgencyCash.API.Notifications.Models;
using AgencyCash.API.Notifications.Services;
using AgencyCash.API.Payments.Constants;
using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IWithdrawalService
{
    Task<PaymentResult> CreateManualWithdrawal(ManualWithdrawalRequest request);
    Task<WithdrawalLimitResult> GetWithdrawalLimitAmount(GetWithdrawalLimitRequest request);
    Task<Result> UpdateTransactionStatus(long transId, UpdateTransactionStatusRequest request);
}

public class WithdrawalService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger,
    INotificationsService notificationsService) : PaymentService(repository, siteDBService, logger), IWithdrawalService
{
    private readonly INotificationsService notificationsService = notificationsService;

    public async Task<PaymentResult> CreateManualWithdrawal(ManualWithdrawalRequest request)
    {
        try
        {
            // Step 1: Get site DB config
            SiteDBConfig siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

            // Step 2: Validate payment
            var validatePaymentCriteria = new ValidatePaymentCriteria(
                request.AgentId,
                request.CustId,
                TransactionType.Withdrawal,
                siteDBConfig.CustDBGroupId,
                isManual: true,
                paymentTo: request.PaymentTo);

            var validateResult = await ValidatePayment(validatePaymentCriteria);

            if (!validateResult.IsSuccess)
            {
                return new PaymentResult(validateResult.Code, validateResult.Message);
            }

            // Step 3: Get withdrawal limits
            var withdrawalLimitCriteria = new WithdrawalLimitAmountCriteria(
                request.CustId,
                request.AgentId,
                isMemberSite: true,
                siteDBConfig.CustDBGroupId);
            await repository.ExecuteAsync(withdrawalLimitCriteria);

            // Step 4: Add withdrawal transaction
            var withdrawalTransactionCriteria = new ManualWithdrawalTransactionCriteria(
                request.CustId,
                request.AgentId,
                siteDBConfig.SiteDBGroupId,
                transactionTypeId: 3,
                withdrawalLimitCriteria.MinAmt,
                withdrawalLimitCriteria.MaxAmt,
                withdrawalLimitCriteria.DailyWithdrawalCnt_Setting,
                request.PaymentTo,
                request.Amount,
                request.IP);

            var logs = await repository.FetchAsync<LogEntry>(withdrawalTransactionCriteria);
            var paymentResult = new PaymentResult(
                withdrawalTransactionCriteria.ErrCode,
                withdrawalTransactionCriteria.ErrCode == 0 ? "Ok" : withdrawalTransactionCriteria.ErrMsg,
                withdrawalTransactionCriteria.TransId);

            await WriteLogsTransferAndRunSP(logs, siteDBConfig);

            if (!string.IsNullOrWhiteSpace(withdrawalTransactionCriteria.ErrExtraInfo))
            {
                await logger.ErrorAsync($"Withdrawal failed. Key: {withdrawalTransactionCriteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(withdrawalTransactionCriteria)}");
            }

            if (paymentResult.IsSuccess)
            {
                _ = Task.Run(() => NotifyPendingWithdrawal(
                    request.AgentId,
                    request.CustId,
                    [request.Amount.ToString("N2"),
                    validateResult.MemberNickname]));
            }

            return paymentResult;
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return new PaymentResult(500, "An error occurred while processing withdrawal request.");
        }
    }

    public async Task<WithdrawalLimitResult> GetWithdrawalLimitAmount(GetWithdrawalLimitRequest request)
    {
        try
        {
            // Step 1: Get site DB config
            SiteDBConfig siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

            // Step 2: Get member cash balance
            var getMemberCashBalanceCriteria = new GetMemberCashBalanceCriteria(request.CustId, siteDBConfig.SiteDBGroupId);
            await repository.ExecuteAsync(getMemberCashBalanceCriteria);

            // Step 3: Get daily withdrawal count
            var getDailyWithdrawalCriteria = new GetDailyWithdrawalCriteria(request.CustId, siteDBConfig.SiteDBGroupId);
            await repository.ExecuteAsync(getDailyWithdrawalCriteria);

            // Step 4: Get withdrawal limits
            var withdrawalLimitCriteria = new WithdrawalLimitAmountCriteria(request.CustId, request.AgentId, isMemberSite: true, siteDBConfig.CustDBGroupId, getDailyWithdrawalCriteria.WithdrawalCnt);

            await repository.ExecuteAsync(withdrawalLimitCriteria);

            return new WithdrawalLimitResult(withdrawalLimitCriteria.MinAmt, withdrawalLimitCriteria.MaxAmt, withdrawalLimitCriteria.WithdrawalLimitCnt, getMemberCashBalanceCriteria.TotalCash);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return new WithdrawalLimitResult();
        }
    }

    public async Task<Result> UpdateTransactionStatus(long transId, UpdateTransactionStatusRequest request)
    {
        var tasks = new List<Task>();
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);

        var criteria = new UpdateWithdrawalTransStatusCriteria(
            request.AgentId,
            request.CustId,
            transId,
            request.StatusId,
            siteDBConfig.SiteDBGroupId,
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
            tasks.Add(logger.ErrorAsync($"Withdrawal - {nameof(UpdateTransactionStatus)} failed. Key: {criteria.GetSettingKey()}, Criteria: {JsonHelper.ToJson(criteria)}"));
        }

        return result;
    }

    public async Task NotifyPendingWithdrawal(int agentId, int custId, object[] messageParams)
    {
        try
        {
            await notificationsService.NotifyPendingWithdrawal(agentId, custId, messageParams);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"{nameof(NotifyPendingWithdrawal)} failed. Agent: {agentId}, CustId: {custId}", ex);
        }
    }
}