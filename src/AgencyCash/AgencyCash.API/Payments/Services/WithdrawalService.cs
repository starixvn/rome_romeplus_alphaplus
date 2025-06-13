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

public class WithdrawalService : PaymentService, IWithdrawalService
{
    private readonly IDynamicRepository _repository;
    private readonly ISiteDBService _siteDBService;

    public WithdrawalService(
        IDynamicRepository repository,
        ISiteDBService siteDBService,
        Fanex.Logging.ILogger logger) : base(repository, siteDBService, logger)
    {
        _repository = repository;
        _siteDBService = siteDBService;
    }

    public async Task<PaymentResult> CreateManualWithdrawal(ManualWithdrawalRequest request)
    {
        try
        {
            // Step 1: Get site DB config
            SiteDBConfig siteDBConfig = await _siteDBService.GetSiteDBConfig(request.AgentId);

            // Step 2: Validate payment
            var validatePaymentCriteria = new ValidatePaymentCriteria(
            request.AgentId,
            request.CustId,
            TransactionType.Withdrawal,
            siteDBConfig.CustDBGroupId,
            isManual: true,
            paymentTo: request.PaymentTo);

            var result = await ValidatePayment(validatePaymentCriteria);

            if (result.Code != 0)
            {
                return result;
            }

            // Step 3: Get withdrawal limits
            var withdrawalLimitCriteria = new WithdrawalLimitAmountCriteria(
                request.CustId,
                request.AgentId,
                isMemberSite: true,
                siteDBConfig.CustDBGroupId);

            await _repository.ExecuteAsync(withdrawalLimitCriteria);

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

            var logs = await _repository.FetchAsync<LogEntry>(withdrawalTransactionCriteria);
            await WriteLogsTransferAndRunSP(logs, siteDBConfig);

            return new PaymentResult(withdrawalTransactionCriteria.ErrCode, withdrawalTransactionCriteria.ErrCode == 0 ? "Ok" : withdrawalTransactionCriteria.ErrMsg, withdrawalTransactionCriteria.TransId);
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
            SiteDBConfig siteDBConfig = await _siteDBService.GetSiteDBConfig(request.AgentId);

            // Step 2: Get member cash balance
            var getMemberCashBalanceCriteria = new GetMemberCashBalanceCriteria(request.CustId, siteDBConfig.SiteDBGroupId);
            await _repository.ExecuteAsync(getMemberCashBalanceCriteria);

            // Step 3: Get daily withdrawal count
            var getDailyWithdrawalCriteria = new GetDailyWithdrawalCriteria(request.CustId, siteDBConfig.SiteDBGroupId);
            await _repository.ExecuteAsync(getDailyWithdrawalCriteria);

            // Step 4: Get withdrawal limits
            var withdrawalLimitCriteria = new WithdrawalLimitAmountCriteria(request.CustId, request.AgentId, isMemberSite: true, siteDBConfig.CustDBGroupId);

            await _repository.ExecuteAsync(withdrawalLimitCriteria);

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
            tasks.Add(logger.ErrorAsync($"Update Withdrawal Transaction Status failed for TransId: {criteria.TransId}, Criteria: {JsonHelper.ToJson(criteria)}"));
        }

        return result;
    }
}