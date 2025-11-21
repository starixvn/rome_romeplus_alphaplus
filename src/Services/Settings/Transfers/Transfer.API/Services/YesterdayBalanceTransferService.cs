using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Models.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface IYesterdayBalanceTransferService
{
    Task<Result> ResetPotentialWinLimit(int custIds, int siteDbGroupId);

    Task<Result> ResetPotentialWinLimit(IEnumerable<int> custIds, int siteDbGroupId);

    Task<Result> ResetPotentialWinLimit(string custIds, int siteDbGroupId);

    Task<MemberTransfer> GetMemberTransfer(int custId, bool allowTotalBalanceTransfer, int siteDbGroupId);

    Task<Result> Transfer(TransferYesterdayBalanceParam param);

    Task<Result> MemberTransfer(AgentTransferYesterdayBalanceParam param);

    Task<Result> DirectMemberTransfer(AgentTransferYesterdayBalanceParam param);

    Task<Result> MultipleMemberTransfer(AgentTransferMultipleYesterdayBalanceCriteria criteria);

    Task<Result> MultipleDirectMemberTransfer(DirectMemberTransferMultipleYesterdayBalanceCriteria criteria);
}

public class YesterdayBalanceTransferService(IDynamicRepository repository) : IYesterdayBalanceTransferService
{
    public async Task<Result> ResetPotentialWinLimit(string custIds, int siteDbGroupId)
    {
        var criteria = new SelectMemberWinLimitCriteria(custIds);
        var membersNeedReset = await repository.FetchAsync<PotentialWinLimitModel>(criteria);

        if (membersNeedReset.Any())
        {
            var ids = membersNeedReset.Select(i => i.CustId).ToList();
            var resetCriteria = new ResetPotentialWinLimit(ids, siteDbGroupId);
            await repository.ExecuteAsync(resetCriteria);
            return new Result { Code = resetCriteria.ErrCode, Message = resetCriteria.ErrMsg };
        }

        return new Result { Code = 0, Message = "Success" };
    }

    public async Task<Result> ResetPotentialWinLimit(IEnumerable<int> custIds, int siteDbGroupId)
    => await ResetPotentialWinLimit(string.Join(",", custIds), siteDbGroupId);

    public async Task<Result> ResetPotentialWinLimit(int custIds, int siteDbGroupId)
    => await ResetPotentialWinLimit(custIds.ToString(), siteDbGroupId);

    public async Task<MemberTransfer> GetMemberTransfer(int custId, bool allowTotalBalanceTransfer, int siteDbGroupId)
    {
        var criteria = new GetMemberTransferInfoCriteria(custId, allowTotalBalanceTransfer, siteDbGroupId);
        await repository.ExecuteAsync(criteria);

        return new MemberTransfer
        {
            CustId = custId,
            Username = criteria.Username,
            CashBalance = criteria.CashBalance,
            Oustanding = criteria.Oustanding,
            GivenCredit = criteria.GivenCredit,
            BetCredit = criteria.BetCredit,
            AmountTransferred = criteria.AmtTransferred,
            LastLogin = criteria.LastLogin,
            Balance = criteria.Balance,
            MaxTransDate = criteria.MaxTransDate,
            PendingTransfer3rd = criteria.PendingTransfer3rd
        };
    }

    public async Task<Result> Transfer(TransferYesterdayBalanceParam param)
    {
        var criteria = BuildTransferYesterdayBalanceCriteria(param);
        await repository.ExecuteAsync(criteria);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }

    private static TransferYesterdayBalanceCriteria BuildTransferYesterdayBalanceCriteria(TransferYesterdayBalanceParam param)
    {
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevelId);

        return custLevel.IsMasterLevel()
            ? new MasterTransferYesterdayBalanceCriteria(
                param.CustId,
                param.ToCustIds,
                param.IsFullTransfer,
                -param.Amount,
                param.CustStatus,
                param.AutoTransfer,
                param.Language,
                param.IP,
                param.SubAccId,
                param.SiteDbGroupId)
            : new SuperTransferYesterdayBalanceCriteria(
                param.CustId,
                param.ToCustIds,
                param.IsFullTransfer,
                -param.Amount,
                param.CustStatus,
                param.AutoTransfer,
                param.Language,
                param.IP,
                param.SubAccId,
                param.SiteDbGroupId);
    }

    public async Task<Result> MemberTransfer(AgentTransferYesterdayBalanceParam param)
    {
        var criteria = new AgentTransferYesterdayBalanceCriteria(
            param.AgentName,
            param.MemberId,
            -param.Amount,
            param.IsAllDayBalanceTransfer,
            param.Language,
            param.IP,
            param.AgentId,
            param.SubAccId,
            param.SiteDbGroupId);
        await repository.ExecuteAsync(criteria);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }

    public async Task<Result> MultipleMemberTransfer(AgentTransferMultipleYesterdayBalanceCriteria criteria)
    {
        await repository.ExecuteAsync(criteria);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }

    public async Task<Result> MultipleDirectMemberTransfer(DirectMemberTransferMultipleYesterdayBalanceCriteria criteria)
    {
        await repository.ExecuteAsync(criteria);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }

    public async Task<Result> DirectMemberTransfer(AgentTransferYesterdayBalanceParam param)
    {
        var criteria = new DirectMemberTransferYesterdayBalanceCriteria(
            param.MemberId,
            param.AgentName,
            -param.Amount,
            param.Language,
            param.IP,
            param.AgentId,
            param.SubAccId,
            param.SiteDbGroupId);
        await repository.ExecuteAsync(criteria);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }
}