using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;
using System;
using System.Threading.Tasks;

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
        /*
        return new PaymentResult(0, string.Empty, 123456789);
        */
        SiteDBConfig siteDBConfig = null;
        try
        {
            // Step 1: Get site DB config
            siteDBConfig = await _siteDBService.GetSiteDBConfig(request.AgentId);

            // Step 2: Validate payment
            var validatePaymentCriteria = new ValidatePaymentCriteria(
                request.AgentId,
                request.CustId,
                1, // TransType for withdrawal
                siteDBConfig.CustDBGroupId);

            await _repository.ExecuteAsync(validatePaymentCriteria);
            if (validatePaymentCriteria.ErrCode != 0)
            {
                return new PaymentResult(validatePaymentCriteria.ErrCode, validatePaymentCriteria.ErrMsg, 0);
            }

            // Step 3: Get withdrawal limits
            var withdrawalLimitCriteria = new WithdrawalLimitAmountCriteria(
                request.CustId,
                request.AgentId,
                true, // IsMemberSite
                siteDBConfig.CustDBGroupId);

            await _repository.ExecuteAsync(withdrawalLimitCriteria);
            if (request.Amount < withdrawalLimitCriteria.MinAmt || request.Amount > withdrawalLimitCriteria.MaxAmt)
            {
                return new PaymentResult(601, $"Amount must be between {withdrawalLimitCriteria.MinAmt} and {withdrawalLimitCriteria.MaxAmt}.", 0);
            }

            // Step 4: Add withdrawal transaction
            var withdrawalTransactionCriteria = new ManualWithdrawalTransactionCriteria(
                request.CustId,
                request.AgentId,
                siteDBConfig.SiteDBGroupId,
                1, // TransactionTypeId for withdrawal
                withdrawalLimitCriteria.MinAmt,
                withdrawalLimitCriteria.MaxAmt,
                withdrawalLimitCriteria.DailyWithdrawalCnt_Setting,
                request.PaymentTo,
                request.Amount,
                request.IP);

            await _repository.ExecuteAsync(withdrawalTransactionCriteria);
            if (withdrawalTransactionCriteria.ErrCode != 0)
            {
                return new PaymentResult(withdrawalTransactionCriteria.ErrCode, withdrawalTransactionCriteria.ErrMsg, 0);
            }

            // Step 5: Insert transfer
            var transferCriteria = new ManualWithdrawalTransferCriteria(
                request.CustId,
                request.AgentId,
                1, // ExchangeRate
                request.IP,
                request.Amount,
                siteDBConfig.SiteDBGroupId);

            await _repository.ExecuteAsync(transferCriteria);
            if (transferCriteria.ErrCode != 0)
            {
                // Log transfer failure
                await WriteLogs(new[] { new LogEntry
                {
                    LogTypeId = 4,
                    UserId = request.AgentId,
                    CustId = request.CustId,
                    FromCustId = request.AgentId,
                    ToCustId = request.CustId,
                    ActionId = 1,
                    TrackingValue = $@"<root>
                        <paymentFrom>{request.PaymentFrom}</paymentFrom>
                        <paymentTo>{request.PaymentTo}</paymentTo>
                        <amount>{request.Amount}</amount>
                        <errCode>{transferCriteria.ErrCode}</errCode>
                        <errMsg>{transferCriteria.ErrMsg}</errMsg>
                    </root>",
                    IP = request.IP,
                    Doing = "Manual withdrawal transfer failed"
                }}, siteDBConfig.CustDBGroupId);

                return new PaymentResult(transferCriteria.ErrCode, transferCriteria.ErrMsg, 0);
            }

            // Step 6: Log successful transfer
            await WriteLogs(new[] { new LogEntry
            {
                LogTypeId = 4,
                UserId = request.AgentId,
                CustId = request.CustId,
                FromCustId = request.AgentId,
                ToCustId = request.CustId,
                ActionId = 1,
                TrackingValue = $@"<root>
                    <paymentFrom>{request.PaymentFrom}</paymentFrom>
                    <paymentTo>{request.PaymentTo}</paymentTo>
                    <amount>{request.Amount}</amount>
                    <transferRefNo>{transferCriteria.TransferRefNo}</transferRefNo>
                </root>",
                IP = request.IP,
                Doing = "Manual withdrawal transfer successful"
            }}, siteDBConfig.CustDBGroupId);

            // Step 7: Log successful transaction
            var writeLogCriteria = new WriteLogRunSPCriteria(
                request.AgentId,
                request.CustId,
                1,
                string.Empty,
                string.Empty,
                $@"<root>
                    <paymentFrom>{request.PaymentFrom}</paymentFrom>
                    <paymentTo>{request.PaymentTo}</paymentTo>
                    <amount>{request.Amount}</amount>
                    <transId>{withdrawalTransactionCriteria.TransId}</transId>
                    <transferRefNo>{transferCriteria.TransferRefNo}</transferRefNo>
                </root>",
                string.Empty);

            await _repository.ExecuteAsync(writeLogCriteria);

            // Step 8: Log to DBLOG database
            var dbLogCriteria = new WriteLogRunSPCriteria(
                request.AgentId,
                request.CustId,
                1,
                string.Empty,
                string.Empty,
                $@"<root>
                    <paymentFrom>{request.PaymentFrom}</paymentFrom>
                    <paymentTo>{request.PaymentTo}</paymentTo>
                    <amount>{request.Amount}</amount>
                    <transId>{withdrawalTransactionCriteria.TransId}</transId>
                    <transferRefNo>{transferCriteria.TransferRefNo}</transferRefNo>
                    <status>success</status>
                </root>",
                string.Empty);

            await _repository.ExecuteAsync(dbLogCriteria);

            // Step 9: Log to SiteDB database
            var siteDBLogCriteria = new WriteLogRunSPCriteria(
                request.AgentId,
                request.CustId,
                1,
                string.Empty,
                string.Empty,
                $@"<root>
                    <paymentFrom>{request.PaymentFrom}</paymentFrom>
                    <paymentTo>{request.PaymentTo}</paymentTo>
                    <amount>{request.Amount}</amount>
                    <transId>{withdrawalTransactionCriteria.TransId}</transId>
                    <transferRefNo>{transferCriteria.TransferRefNo}</transferRefNo>
                    <status>success</status>
                </root>",
                string.Empty)
            {
                CustDBGroupId = siteDBConfig.CustDBGroupId
            };

            await _repository.ExecuteAsync(siteDBLogCriteria);

            return new PaymentResult(0, string.Empty, withdrawalTransactionCriteria.TransId);
        }
        catch (Exception ex)
        {
            if (siteDBConfig != null)
            {
                // Log error to both DBLOG and SiteDB
                var errorLogEntry = new LogEntry
                {
                    LogTypeId = 4,
                    UserId = request.AgentId,
                    CustId = request.CustId,
                    FromCustId = request.AgentId,
                    ToCustId = request.CustId,
                    ActionId = 1,
                    TrackingValue = $@"<root>
                        <paymentFrom>{request.PaymentFrom}</paymentFrom>
                        <paymentTo>{request.PaymentTo}</paymentTo>
                        <amount>{request.Amount}</amount>
                        <errCode>500</errCode>
                        <errMsg>An error occurred while processing withdrawal request.</errMsg>
                    </root>",
                    IP = request.IP,
                    Doing = $"Manual withdrawal error: {ex.Message}"
                };

                // Log to SiteDB
                await WriteLogs(new[] { errorLogEntry }, siteDBConfig.CustDBGroupId);

                // Log to DBLOG
                var dbLogErrorCriteria = new WriteLogRunSPCriteria(
                    request.AgentId,
                    request.CustId,
                    1,
                    string.Empty,
                    string.Empty,
                    $@"<root>
                        <paymentFrom>{request.PaymentFrom}</paymentFrom>
                        <paymentTo>{request.PaymentTo}</paymentTo>
                        <amount>{request.Amount}</amount>
                        <errCode>500</errCode>
                        <errMsg>An error occurred while processing withdrawal request.</errMsg>
                        <status>error</status>
                    </root>",
                    string.Empty);

                await _repository.ExecuteAsync(dbLogErrorCriteria);
            }

            await logger.ErrorAsync(ex.Message, ex);
            return new PaymentResult(500, "An error occurred while processing withdrawal request.", 0);
        }
    }
}