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
                2, // TransType for withdrawal
                siteDBConfig.CustDBGroupId);

            if (!validatePaymentCriteria.IsValid())
            {
                return new PaymentResult(400, "Bad Request", 0);
            }

            await _repository.ExecuteAsync(validatePaymentCriteria);
            if (validatePaymentCriteria.ErrCode != 0)
            {
                return new PaymentResult(600, "Withdrawal validation failed");
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
                return new PaymentResult(601, $"Amount must be between {withdrawalLimitCriteria.MinAmt} and {withdrawalLimitCriteria.MaxAmt}.");
            }

            // Step 4: Add withdrawal transaction
            var withdrawalTransactionCriteria = new ManualWithdrawalTransactionCriteria(
                request.CustId,
                request.AgentId,
                siteDBConfig.SiteDBGroupId,
                3, // TransactionTypeId for withdrawal
                withdrawalLimitCriteria.MinAmt,
                withdrawalLimitCriteria.MaxAmt,
                withdrawalLimitCriteria.DailyWithdrawalCnt_Setting,
                request.PaymentTo,
                request.Amount,
                request.IP);

            await _repository.ExecuteAsync(withdrawalTransactionCriteria);

            // Step 5.1: Log transfer success
            if (withdrawalTransactionCriteria.ErrCode == 0)
            {
                // Log transfer success
                /*TODOawait WriteLogs(new[] { new LogEntry
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
                    Doing = "Manual withdrawal transfer successful"
                }}, siteDBConfig.CustDBGroupId);*/
            }

            // Step 5.2: Log Log transfer when step 5.1 is not success
            /*TODOawait WriteLogs(new[] { new LogEntry
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

            // Step 6.1: Purpose : Insert Log when step 3 success
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

            // Step 6.2: Purpose : Insert Log When step 6.1  failed
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

            await _repository.ExecuteAsync(dbLogCriteria);*/

            if (withdrawalTransactionCriteria.ErrCode != 0)
            {
                return new PaymentResult(602, "Withdrawal insert transaction failed");
            }

            return new PaymentResult(0, string.Empty, withdrawalTransactionCriteria.TransId);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync(ex.Message, ex);
            return new PaymentResult(500, "An error occurred while processing withdrawal request.");
        }
    }
}