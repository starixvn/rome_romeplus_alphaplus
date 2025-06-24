namespace AlphaPlus.CashService.API.Features.PendingDeposit.Services;

using AlphaPlus.CashService.API.Features.Shared.Models;
using AlphaPlus.CashService.API.Features.Transactions.Models;
using AlphaPlus.CashService.API.Features.Transactions.Services;
using Criteria;
using Fanex.Data.Repository;
using Models;

public interface IPendingDepositService
{
    Task<decimal> GetLimitAmtDepositAsync(int agentId, int custId, int custDbGroupId);

    Task<MemberSettingDeposit> GetMemberSettingDepositAsync(int custId, int custDbGroupId);

    Task<MemberSettingDeposit> GetMemberSettingDepositWhenProcessTransactionAsync(int custId, int agentId,
        int custDbGroupId);

    Task<ResultModel> AddManualDepositAsync(CreateManualDepositModel param);

    Task<ResultModel> ProcessDepositAsync(ProcessDepositTransaction param);
}

public class PendingDepositService(IDynamicRepository dynamicRepository,
    ITransactionService transactionService) : IPendingDepositService
{
    public async Task<decimal> GetAccumulateCreditAsync(int agentId)
    {
        var criteria = new GetAccumulateCreditCriteria
        {
            CustId = agentId
        };
        await dynamicRepository.GetAsync<decimal>(criteria);

        return criteria.AccumCreditReturn;
    }

    public async Task<decimal> GetLimitAmtDepositAsync(int agentId, int custId, int custDbGroupId)
    {
        var acCumCreditReturn = await GetAccumulateCreditAsync(agentId);

        var criteria = new GetLimitAmtDepositCriteria
        {
            CustId = custId,
            AccumCreditReturn = acCumCreditReturn
        };
        await dynamicRepository.GetAsync<CreditAmount>(criteria);

        return criteria.AgentAvailableCredit;
    }

    public async Task<MemberSettingDeposit> GetMemberSettingDepositWhenProcessTransactionAsync(int custId, int agentId, int custDbGroupId)
    {
        var memberSetting = new MemberSettingDeposit();

        var criteria = new GetLimitAmtDepositCriteria
        {
            CustId = custId,
            AgentId = agentId,
            CustDbGroupId = custDbGroupId
        };
        await dynamicRepository.GetAsync<CreditAmount>(criteria);
        memberSetting.AgentGivenCredit = criteria.AgentGivenCredit;
        memberSetting.CreditMembers_GivenCredit = criteria.CreditMembers_GivenCredit;
        return memberSetting;
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

        return memberSetting;
    }

    public async Task<ResultModel> AddManualDepositAsync(CreateManualDepositModel param)
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
            PaymentFrom = param.PaymentFrom,
            PaymentTo = param.PaymentTo,
            Amount = param.Amount,
            PaySlip = param.PaySlip,
            Ip = param.Ip,
            Remark = param.Remark
        };

        var logs = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);

        transactionService.OnAfterUpdateTransaction(command.CashAccumCreditTbl, param.SiteDbGroupId, param.CustDbGroupId, logs.ToList());

        return result;
    }

    public async Task<ResultModel> ProcessDepositAsync(ProcessDepositTransaction param)
    {
        var command = new ProcessDepositCommand
        {
            TransId = param.TransId,
            SiteDbGroupId = param.SiteDbGroupId,
            CustId = param.CustId,
            AgentId = param.ActorId,
            SubAccId = param.SubAccId,
            NewAmount = param.NewAmount,
            AgentGivenCredit = param.AgentGivenCredit,
            CreditMembersGivenCredit = param.CreditMembersGivenCredit,
            StatusId = param.StatusId,
            Ip = param.Ip,
            Remark = param.Remark
        };
        var logs = await dynamicRepository.FetchAsync<WriteLogDBCommandModel>(command);
        var result = new ResultModel(command.ErrCode, command.ErrMsg);

        transactionService.OnAfterUpdateTransaction(command.CashAccumCreditTbl, param.SiteDbGroupId, param.CustDbGroupId, logs.ToList());

        return result;
    }
}