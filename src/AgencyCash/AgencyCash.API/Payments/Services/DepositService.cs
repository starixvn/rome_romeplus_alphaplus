using AgencyCash.API.Payments.Constants;
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
        var validateResult = await ValidatePayment(request.AgentId, request.CustId, TransactionType.Deposit, siteDBConfig.CustDBGroupId);

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
            request.PaySlipBuffer,
            request.IP,
            request.Remark);

        return await AddTransactionAndWriteLogs(criteria, siteDBConfig.CustDBGroupId);
    }

    public async Task<Result> Auto(AutoDepositRequest request)
    {
        var siteDBConfig = await siteDBService.GetSiteDBConfig(request.AgentId);
        var validateResult = await ValidatePayment(request.AgentId, request.CustId, TransactionType.Deposit, siteDBConfig.CustDBGroupId);

        if (!validateResult.IsSuccess)
        {
            return validateResult;
        }

        var limitCriteria = new GetDepositLimitCriteria(
            request.AgentId,
            request.CustId,
            paymentType: request.DepositMethod,
            providerId: request.ProviderId ?? 0,
            paymentMethodId: request.PaymentMethodId,
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
            transactionTypeId: TransactionType.AutoDeposit,
            paymentType: request.DepositMethod,
            paymentMethodId: request.PaymentMethodId,
            request.PaymentFrom,
            request.PaymentTo,
            request.Amount,
            paySlip: null,
            request.IP,
            request.Remark);

        return await AddTransactionAndWriteLogs(criteria, siteDBConfig.CustDBGroupId);
    }

    private async Task<Result> AddTransactionAndWriteLogs(AddTransactionDepositCriteria criteria, short custDBGroupId)
    {
        var logs = await repository.FetchAsync<LogEntry>(criteria);
        _ = Task.Run(async () => await WriteLogs(logs, custDBGroupId));

        return new PaymentResult(criteria.ErrCode, criteria.ErrMsg, criteria.TransId);
    }
}