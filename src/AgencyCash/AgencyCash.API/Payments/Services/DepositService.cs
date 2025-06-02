using AgencyCash.API.Payments.Criteria.Deposit;
using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Shared;
using AgencyCash.API.Shared.Models;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Services;

public interface IDepositService
{
    Task<DepositLimit> GetLimit(GetDepositLimitRequest request);

    Task<Result> Manual(ManualDepositRequest request);

    Task<Result> Auto(AutoDepositRequest request);
}

public class DepositService(
    IDynamicRepository repository,
    ISiteDBService siteDBService,
    Fanex.Logging.ILogger logger) : PaymentService(repository, siteDBService, logger), IDepositService
{
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

    public async Task<Result> Manual(ManualDepositRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);
        var validateResult = await ValidatePayment(request.AgentId, request.CustId, Deposit, siteDBConfig.CustDBGroupId);

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
            Deposit,
            BankAccount,
            request.PaymentMethodId,
            request.PaymentFrom,
            request.PaymentTo,
            request.Amount,
            request.PaySlipBuffer,
            request.IP,
            request.Remark);
        var logs = await repository.FetchAsync<LogEntry>(criteria);
        var result = new PaymentResult(criteria.ErrCode, criteria.ErrMsg, criteria.TransId);

        if (result.IsSuccess)
        {
            _ = Task.Run(async () => await WriteLogs(logs, siteDBConfig.CustDBGroupId));
        }

        return result;
    }

    public async Task<Result> Auto(AutoDepositRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);
        var validateResult = await ValidatePayment(request.AgentId, request.CustId, Deposit, siteDBConfig.CustDBGroupId);

        if (!validateResult.IsSuccess)
        {
            return validateResult;
        }

        const byte Gateway = 1;
        var limitCriteria = new GetDepositLimitCriteria(
            request.AgentId,
            request.CustId,
            paymentType: Gateway,
            providerId: request.ProviderId ?? 0,
            paymentMethodId: request.PaymentMethodId,
            custDBGroupId: siteDBConfig.CustDBGroupId);
        await repository.ExecuteAsync(limitCriteria);

        // var criteria = new AddTransactionDepositCriteria(
        //     request.AgentId,
        //     request.CustId,
        //     siteDBConfig.SiteDBGroupId,
        //     limitCriteria.AgentGivenCredit,
        //     limitCriteria.CreditMembers_GivenCredit,
        //     limitCriteria.MinAmt,
        //     limitCriteria.MaxAmt,
        //     Deposit,
        //     Gateway,
        //     request.PaymentMethodId,
        //     request.PaymentFrom,
        //     request.PaymentTo,
        //     request.Amount,
        //     request.PaySlipBuffer,
        //     request.IP,
        //     request.Remark);

        throw new NotImplementedException("Auto deposit is not implemented yet.");
    }
}