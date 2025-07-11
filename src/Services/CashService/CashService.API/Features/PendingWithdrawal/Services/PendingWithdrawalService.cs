namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;

using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Criteria;
using Fanex.Data.Repository;
using Models;
using Models.PaymentGateway;

public interface IPendingWithdrawalService
{
    Task<decimal> GetCashBalanceAsync(int custId, int siteDbGroupId);

    Task<CreditAmount> GetLimitAmtWithdrawAsync(GetCashLimitAmt param);

    Task<MemberSettingWithdraw> GetMemberSettingWithdrawAsync(int custId);

    Task<ResultModel> AddManualWithdrawAsync(AddManualWithdrawalModel param);

    Task<ResultModel> ApproveByPaymentGatewayAsync(ProcessWithdrawalTransaction command);

    Task<ResultModel> ProcessWithdrawAsync(ProcessWithdrawalTransaction param);
}

public class PendingWithdrawalService(
    IDynamicRepository dynamicRepository,
    ITransactionService transactionService,
    IPaymentGatewayService paymentGateway) : IPendingWithdrawalService
{
    public async Task<decimal> GetCashBalanceAsync(int custId, int siteDbGroupId)
    {
        var criteria = new GetCashBalanceCriteria
        {
            CustId = custId,
            SiteDbGroupId = siteDbGroupId
        };
        await dynamicRepository.ExecuteAsync(criteria);
        return criteria.TotalCash;
    }

    public async Task<CreditAmount> GetLimitAmtWithdrawAsync(GetCashLimitAmt param)
    {
        var criteria = new GetLimitAmtWithdrawCriteria
        {
            CustId = param.CustId,
            ProviderId = param.ProviderId,
            UplineId = param.UplineId,
            CustDbGroupId = param.CustDbGroupId,
            PaymentMethodId = param.PaymentMethodId
        };
        await dynamicRepository.GetAsync<CreditAmount>(criteria);
        return new CreditAmount
        {
            MaxAmt = criteria.MaxAmt,
            MinAmt = criteria.MinAmt,
        };
    }

    public async Task<MemberSettingWithdraw> GetMemberSettingWithdrawAsync(int custId)
    {
        var criteria = new GetMemberSettingWithdrawCriteria
        {
            CustId = custId
        };
        return await dynamicRepository.GetAsync<MemberSettingWithdraw>(criteria);
    }

    public async Task<ResultModel> AddManualWithdrawAsync(AddManualWithdrawalModel param)
    {
        var memberSettings = new CreditAmount
        {
            MinAmt = null,
            MaxAmt = null
        };

        if (param.ProviderId > 0)
        {
            memberSettings = await GetLimitAmtWithdrawAsync(
                new GetCashLimitAmt
                {
                    CustId = param.CustId,
                    CustDbGroupId = param.CustDbGroupId,
                    UplineId = param.UplineId,
                    ProviderId = param.ProviderId,
                    PaymentMethodId = param.PaymentMethodId
                });
        }

        var command = new CreateManualWithdrawCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            SiteDbGroupId = param.SiteDbGroupId,
            MinLimitAmt = memberSettings.MinAmt,
            MaxLimitAmt = memberSettings.MaxAmt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark,
            ProviderId = param.ProviderId,
            PaymentMethodId = param.PaymentMethodId,
        };
        var responseLogs = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command) ?? [];

        var result = new ResultModel(command.ErrCode, command.ErrMsg);

        transactionService.OnAfterUpdateTransaction(command.CashAccumCreditTbl, param.SiteDbGroupId, param.CustDbGroupId, responseLogs.ToList());

        return result;
    }

    public async Task<ResultModel> ApproveByPaymentGatewayAsync(ProcessWithdrawalTransaction command)
    {
        var result = new ResultModel(400, "Invalid parameters");

        var transaction = await transactionService.GetTransactionByIdAsync(command.TransId, command.CustId, command.SiteDBGroupId);
        var parts = transaction?.PaymentTo?.Split('-') ?? [];
        var bankCode = parts.ElementAtOrDefault(0) ?? string.Empty;
        var accountNumber = parts.ElementAtOrDefault(1) ?? string.Empty;

        if (transaction is null || !string.IsNullOrWhiteSpace(bankCode) || !string.IsNullOrWhiteSpace(accountNumber))
        {
            return result;
        }

        var model = new WithdrawalBodyRequest
        {
            AgentId = command.ActorId,
            CustId = command.CustId,
            Amount = transaction.Amount,
            TransId = command.TransId,
            ProviderId = command.ProviderId,
            MemberInfo = new WithdrawalMemberInfo
            {
                Name = transaction.Username,
                BankCode = bankCode,
                AccountNumber = accountNumber
            },
            ApiConfig = await paymentGateway.GetAgentPaymentGatewayInfo(command.ActorId, transaction.TransactionTypeId, command.ProviderId, command.PaymentMethodId)
        };
        try
        {
            var response = await paymentGateway.WithdrawalViaPaymentGateway(model);

            if (!response.IsSuccessful)
            {
                result.Code = response.Code;
                result.Message = response.Message;
                command.StatusId = 0;
            }
            else
            {
                result = await UpdateProviderTransId(command.TransId, response.Data.ProviderTransId);
            }
        }
        catch (Exception e)
        {
            result.Code = 500;
            result.Message = e.Message;
        }
        return result;
    }

    private async Task<ResultModel> UpdateProviderTransId(long commandTransId, string dataProviderTransId)
    {
        throw new NotImplementedException();
    }

    public async Task<ResultModel> ProcessWithdrawAsync(ProcessWithdrawalTransaction param)
    {
        var command = new ProcessWithdrawalTransactionCommand
        {
            TransId = param.TransId,
            PaymentFrom = param.PaymentFrom,
            CustId = param.CustId,
            ProviderId = param.ProviderId,
            PaymentMethodId = param.PaymentMethodId,
            AgentId = param.ActorId,
            SubAccId = param.SubAccId,
            SiteDbGroupId = param.SiteDBGroupId,
            Remark = param.Remark ?? string.Empty,
            StatusId = param.StatusId,
            Payslip = param.IsRemovePayslip ? [] : param.Payslip,
            Ip = param.IP,
        };
        var responseLogs = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command) ?? [];

        var result = new ResultModel(command.ErrCode, command.ErrMsg);

        transactionService.OnAfterUpdateTransaction(command.CashAccumCreditTbl, param.SiteDBGroupId, param.CustDBGroupId, responseLogs.ToList());

        return result;
    }
}