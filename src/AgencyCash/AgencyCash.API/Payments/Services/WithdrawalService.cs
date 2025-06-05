using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Criteria.Withdrawal;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IWithdrawalService
{
    Task<PaymentResult> CreateManualWithdrawal(ManualWithdrawalRequest request);
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
            transType: 2,
            siteDBConfig.CustDBGroupId,
            isManual: true,
            paymentTo: request.PaymentTo);

            await _repository.ExecuteAsync(validatePaymentCriteria);
            if (validatePaymentCriteria.ErrCode != 0)
            {
                return new PaymentResult(validatePaymentCriteria.ErrCode, validatePaymentCriteria.ErrMsg);
            }

            // Step 3: Get withdrawal limits
            var withdrawalLimitCriteria = new WithdrawalLimitAmountCriteria(
                request.CustId,
                request.AgentId,
                isMemberSite: true,
                siteDBConfig.CustDBGroupId);

            await _repository.ExecuteAsync(withdrawalLimitCriteria);
            if (withdrawalLimitCriteria.ErrCode != 0)
            {
                return new PaymentResult(withdrawalLimitCriteria.ErrCode, withdrawalLimitCriteria.ErrMsg);
            }

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

            var (firstResultSet, _) = await _repository.FetchMultipleAsync<LogEntry, ManualWithdrawalTransactionCriteria>(withdrawalTransactionCriteria);

            if (withdrawalTransactionCriteria.ErrCode == 0)
            {
                await ProcessWithdrawalLogs(firstResultSet.ToList(), siteDBConfig);
            }

            return new PaymentResult(withdrawalTransactionCriteria.ErrCode, withdrawalTransactionCriteria.ErrCode == 0 ? "Ok" : withdrawalTransactionCriteria.ErrMsg, withdrawalTransactionCriteria.TransId);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return new PaymentResult(500, "An error occurred while processing withdrawal request.");
        }
    }

    private async Task ProcessWithdrawalLogs(IList<LogEntry> logs, SiteDBConfig siteDBConfig)
    {
        foreach (var log in logs)
        {
            if (log.LogTypeId == 1)
            {
                // Step 5.1: Log transfer success
                // Step 5.2: Log transfer failure

                await WriteLogsTransfer(logs, siteDBConfig.SiteDBGroupId);
            }
            else
            {
                // Step 6.1: Log run SP success
                // Step 6.2: Log run SP failure

                await WriteLogsRunSP(logs, siteDBConfig.CustDBGroupId);
            }
        }
    }
}