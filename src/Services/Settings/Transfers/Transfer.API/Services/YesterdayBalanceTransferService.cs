using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Models.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface IYesterdayBalanceTransferService
{
    Task<Result> ResetPotentialWinLimit(IEnumerable<int> custIds, string agIP);

    Task<Result> ResetPotentialWinLimit(string custIds, string agIP);

    Task<MemberTransfer> GetMemberTransfer(int custId, bool allowTotalBalanceTransfer, string agIP);

    Task<Result> Transfer(TransferYesterdayBalanceParam param);

    Task<Result> MemberTransfer(AgentTransferYesterdayBalanceParam param);

    Task<Result> DirectMemberTransfer(AgentTransferYesterdayBalanceParam param);

    Task<Result> MultipleMemberTransfer(AgentTransferMultipleYesterdayBalanceCriteria criteria, string agIP);

    Task<Result> MultipleDirectMemberTransfer(DirectMemberTransferMultipleYesterdayBalanceCriteria criteria, string agIP);
}

public class YesterdayBalanceTransferService(IDynamicRepository repository) : IYesterdayBalanceTransferService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<Result> ResetPotentialWinLimit(string custIds, string agIP)
    {
        var criteria = new SelectMemberWinLimitCriteria(custIds);
        var membersNeedReset = await repository.FetchAsync<PotentialWinLimitModel>(criteria, agIP);

        if (membersNeedReset != null)
        {
            var resetCriteria = new ResetPotentialWinLimit(membersNeedReset.Select(i => i.CustId));
            await repository.ExecuteAsync(resetCriteria, agIP);
            return new Result { Code = resetCriteria.ErrCode, Message = resetCriteria.ErrMsg };
        }

        return new Result { Code = 0, Message = "Success" };
    }

    public async Task<Result> ResetPotentialWinLimit(IEnumerable<int> custIds, string agIP)
    => await ResetPotentialWinLimit(string.Join(",", custIds), agIP);

    public async Task<MemberTransfer> GetMemberTransfer(int custId, bool allowTotalBalanceTransfer, string agIP)
    {
        var criteria = new GetMemberTransferInfoCriteria(custId, allowTotalBalanceTransfer);
        await repository.ExecuteAsync(criteria, agIP);

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
        await repository.ExecuteAsync(criteria, param.AGIP);

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
                param.SubAccId)
            : new SuperTransferYesterdayBalanceCriteria(
                param.CustId,
                param.ToCustIds,
                param.IsFullTransfer,
                -param.Amount,
                param.CustStatus,
                param.AutoTransfer,
                param.Language,
                param.IP,
                param.SubAccId);
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
            param.SubAccId);
        await repository.ExecuteAsync(criteria, param.AGIP);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }

    public async Task<Result> MultipleMemberTransfer(AgentTransferMultipleYesterdayBalanceCriteria criteria, string agIP)
    {
        await repository.ExecuteAsync(criteria, agIP);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }

    public async Task<Result> MultipleDirectMemberTransfer(DirectMemberTransferMultipleYesterdayBalanceCriteria criteria, string agIP)
    {
        await repository.ExecuteAsync(criteria, agIP);

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
            param.SubAccId);
        await repository.ExecuteAsync(criteria, param.AGIP);

        return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
    }
}
