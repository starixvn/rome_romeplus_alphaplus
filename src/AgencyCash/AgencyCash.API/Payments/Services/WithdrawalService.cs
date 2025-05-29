using AgencyCash.API.Payments.Criteria;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IWithdrawalService
{
    Task<Result> CreateManualWithdrawal(ManualWithdrawalRequest request);
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

    public async Task<Result> CreateManualWithdrawal(ManualWithdrawalRequest request)
    {
        SiteDBConfig siteDBConfig = null;
        try
        {
            siteDBConfig = await _siteDBService.GetSiteDBConfig(request.AgentId);

            var isEnableCashService = await IsEnableCashService(request.AgentId, siteDBConfig.CustDBGroupId);
            if (!isEnableCashService)
            {
                return new Result(601, "Cash service is not enabled for this agent.");
            }

            var memberSetting = await GetMemberSetting(request.CustId, siteDBConfig.CustDBGroupId);
            if (request.Amount < memberSetting.MinLimitAmount || request.Amount > memberSetting.MaxLimitAmount)
            {
                return new Result(602, $"Amount must be between {memberSetting.MinLimitAmount} and {memberSetting.MaxLimitAmount}.");
            }

            if (request.Amount > memberSetting.AgentGivenCredit)
            {
                return new Result(603, $"Amount cannot exceed agent given credit of {memberSetting.AgentGivenCredit}.");
            }

            var manualWithdrawalCriteria = new ManualWithdrawalCriteria(
                request.CustId,
                request.AgentId,
                memberSetting.AgentGivenCredit,
                memberSetting.CreditMembersGivenCredit,
                memberSetting.MinLimitAmount,
                memberSetting.MaxLimitAmount,
                request.PaymentFrom,
                request.PaymentTo,
                1,
                request.Amount,
                request.IP,
                siteDBConfig.SiteDBGroupId);

            await _repository.ExecuteAsync(manualWithdrawalCriteria);
            var result = new Result(
                manualWithdrawalCriteria.ErrCode,
                manualWithdrawalCriteria.ErrMsg);

            if (result.IsSuccess)
            {
                SyncCashAccumCredit(
                    manualWithdrawalCriteria.CashAccumCreditTbl,
                    siteDBConfig.CustDBGroupId);
            }

            await WriteLogs([ new LogEntry {
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
                </root>",
                IP = request.IP,
                Doing = "Manual withdrawal"
            } ], siteDBConfig);

            return result;
        }
        catch (Exception ex)
        {
            if (siteDBConfig != null)
            {
                await WriteLogs([ new LogEntry {
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
                    </root>",
                    IP = request.IP,
                    Doing = $"Manual withdrawal error: {ex.Message}"
                } ], siteDBConfig);
            }

            await logger.ErrorAsync(ex.Message, ex);
            return new Result(604, "An error occurred while processing withdrawal request.");
        }
    }
}