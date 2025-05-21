namespace AlphaPlus.CashService.API.Services;

using System.Numerics;
using AlphaPlus.CashService.API.Criteria.Transactions;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Criteria.PendingWithdrawal;
using Fanex.Data.Repository;
using Models.PendingCommons;
using Models.Transactions;

public interface IPendingWithdrawalService
{
    Task<decimal> GetCashBalanceAsync(int custId, int siteDbGroupId);

    Task<CreditAmount> GetLimitAmtWithdrawAsync(CashLimitAmt param);

    Task<MemberSettingWithdraw> GetMemberSettingWithdrawAsync(int custId);

    Task<ResultModel> AddManualWithdrawAsync(AddManualWithdrawalModel param);

    Task<ResultModel> ProcessWithdrawAsync(ProcessTransactionModel param);
}

public class PendingWithdrawalService(
    IDynamicRepository dynamicRepository,
    ICommonService commonService,
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

    public async Task<CreditAmount> GetLimitAmtWithdrawAsync(CashLimitAmt param)
    {
        var criteria = new GetLimitAmtWithdrawCriteria
        {
            CustId = param.CustId,
            ProviderId = param.ProviderId,
            UplineId = param.UplineId,
            CustDbGroupId = param.CustDbGroupId
        };
        await dynamicRepository.GetAsync<CreditAmount>(criteria);
        var random = new Random();
        var chance = random.Next(0, 10);
        return new CreditAmount
        {
            MaxAmt = chance == 0 ? null : random.Next(0, 100),
            //MaxAmt = criteria.MaxAmt,
            MinAmt = criteria.MinAmt
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
        var memberSettings = await GetLimitAmtWithdrawAsync(
            new CashLimitAmt
            {
                CustId = param.CustId,
                CustDbGroupId = param.CustDbGroupId,
                UplineId = param.UplineId,
                ProviderId = param.ProviderId
            });
        var command = new CreateManualWithdrawCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            MinLimitAmt = memberSettings.MinAmt,
            MaxLimitAmt = memberSettings.MaxAmt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark,
            ProviderId = param.ProviderId,
            SiteDbGroupId = param.SiteDbGroupId
        };
        var logResoponse = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);
        if (command.ErrCode == 0)
        {
            SyncCashAccumulatedCredit(command.CashAccumCreditTbl, param.CustDbGroupId);
        }
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

    private void SyncCashAccumulatedCredit(string cashAccumulatedCreditTbl, int custDbGroupId)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await transactionService.SyncCashAccumCreditAsync(cashAccumulatedCreditTbl, custDbGroupId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
    }
}