using AlphaPlus.CashService.API.Models.Transactions;

namespace AlphaPlus.CashService.API.Services;

using AlphaPlus.CashService.API.Criteria.Transactions;
using System.Numerics;
using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Shared.Services;
using Criteria.PendingDeposit;
using Fanex.Data.Repository;
using Models.PendingCommons;

public interface IPendingDepositService
{
    Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo();

    Task<CreditAmount> GetLimitAmtDepositAsync(int agentId, int custId, int custDbGroupId);

    Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, int custDbGroupId);

    Task<ResultModel> AddManualDepositAsync(CreateManualModel param);

    Task<ResultModel> ProcessDepositAsync(ProcessTransactionModel param);
}

public class PendingDepositService(IDynamicRepository dynamicRepository,
    ICommonService commonService,
    ITransactionService transactionService) : IPendingDepositService
{
    public async Task<IEnumerable<BankAccountInfo>> GetMemberBankAccountInfo()
        => await dynamicRepository.FetchAsync<BankAccountInfo>(new GetMemberBankAccountCriteria());

    public async Task<decimal> GetAccumulateCreditAsync(int agentId)
    {
        var criteria = new GetAccumulateCreditCriteria
        {
            CustId = agentId
        };
        await dynamicRepository.GetAsync<decimal>(criteria);

        return criteria.AccumCreditReturn;
    }

    public async Task<CreditAmount> GetLimitAmtDepositAsync(int agentId, int custId, int custDbGroupId)
    {
        var creditAmount = new CreditAmount();
        var acCumCreditReturn = await GetAccumulateCreditAsync(agentId);

        var criteria = new GetLimitAmtDepositCriteria
        {
            CustId = custId,
            AccumCreditReturn = acCumCreditReturn
        };
        await dynamicRepository.GetAsync<CreditAmount>(criteria);
        creditAmount.MaxAmt = criteria.MaxAmt;
        creditAmount.MinAmt = criteria.MinAmt;

        return creditAmount;
    }

    public async Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, int custDbGroupId)
    {
        var memberSetting = new MemberSettingDeposit();
        var criteria = new GetMemberSettingDepositCriteria
        {
            CustId = custId,
            CustDbGroupId = custDbGroupId
        };
        await dynamicRepository.GetAsync<MemberSettingDeposit>(criteria);
        memberSetting.AgentGivenCredit = criteria.AgentGivenCredit;
        memberSetting.CreditMembers_GivenCredit = criteria.CreditMembers_GivenCredit;
        memberSetting.MinLimitAmt = criteria.MinLimitAmt;
        memberSetting.MaxLimitAmt = criteria.MaxLimitAmt;

        return memberSetting;
    }

    public async Task<ResultModel> AddManualDepositAsync(CreateManualModel param)
    {
        var memberSettings = await GetMemberSettingDepositAsync(param.CustId, param.CustDbGroupId);
        var command = new CreateManualDepositCommand
        {
            CustId = param.CustId,
            UplineId = param.UplineId,
            SubAccId = param.SubAccId,
            SiteDbGroupId = param.SiteDbGroupId,
            AgentGivenCredit = memberSettings.AgentGivenCredit,
            CreditMembers_GivenCredit = memberSettings.CreditMembers_GivenCredit,
            MinLimitAmt = memberSettings.MinLimitAmt,
            MaxLimitAmt = memberSettings.MaxLimitAmt,
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark
        };

        await dynamicRepository.ExecuteAsync(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);
        if (command.ErrCode == 0)
        {
            SyncCashAccumulatedCredit(command.CashAccumCreditTbl, param.CustDbGroupId);
        }
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

    public async Task<ResultModel> ProcessDepositAsync(ProcessTransactionModel param)
    {
        var command = new ProcessDepositCommand
        {
            TransId = BigInteger.Parse(param.TransId),
            Amount = param.Amount,
            Remark = param.Remark,
            StatusId = param.StatusId
        };
        await dynamicRepository.ExecuteAsync(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);
        return result;
    }
}