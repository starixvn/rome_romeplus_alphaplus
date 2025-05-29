namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;

using System.Numerics;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Transactions.Criteria;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Criteria;
using Fanex.Data.Repository;
using Models;

public interface IPendingWithdrawalService
{
    Task<decimal> GetCashBalanceAsync(int custId, int siteDbGroupId);

    Task<CreditAmount> GetLimitAmtWithdrawAsync(GetCashLimitAmt param);

    Task<MemberSettingWithdraw> GetMemberSettingWithdrawAsync(int custId);

    Task<ResultModel> AddManualWithdrawAsync(AddManualWithdrawalModel param);

    Task<ResultModel> ProcessWithdrawAsync(ProcessTransactionModel param);
}

public class PendingWithdrawalService(
    IDynamicRepository dynamicRepository,
    ITransactionService transactionService) : IPendingWithdrawalService
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

    public async Task<ResultModel> ProcessWithdrawAsync(ProcessTransactionModel param)
    {
        var command = new ProcessTransactionCommand
        {
            TransId = BigInteger.Parse(param.TransId),
            PaymentFrom = param.PaymentFrom,
            Remark = param.Remark,
            StatusId = param.StatusId,
            Payslip = param.Payslip
        };
        await dynamicRepository.ExecuteAsync(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);
        return result;
    }
}