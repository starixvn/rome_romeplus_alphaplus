using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Models.TransferYesterdayBalance;
using AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ITransferYesterdayBalanceService
{
    Task<MemberTransfer> GetMemberTransfer(int custId, bool allowTotalBalanceTransfer, string agIP);

    Task<Result> Transfer(TransferYesterdayBalanceParam param);
}

public class TransferYesterdayBalanceService(IDynamicRepository repository) : ITransferYesterdayBalanceService
{
    private readonly IDynamicRepository repository = repository;

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

        if (custLevel.IsAgentLevel())
        {
            return new AgentTransferYesterdayBalanceCriteria(
                param.CustId,
                param.SuperIdsTransferTotal,
                param.ToCustIds,
                param.Amount,
                param.IsAllDayBalanceTransfer,
                param.AutoTransfer,
                param.Language,
                param.IP,
                param.CustId,
                param.SubAccId);
        }

        if (custLevel.IsMasterLevel())
        {
            return new MasterTransferYesterdayBalanceCriteria(
                param.CustId,
                param.SuperIdsTransferTotal,
                param.ToCustIds,
                param.IsFullTransfer,
                param.Amount,
                param.CustStatus,
                param.AutoTransfer,
                param.Language,
                param.IP,
                param.CustId,
                param.SubAccId);
        }

        return new SuperTransferYesterdayBalanceCriteria(
            param.CustId,
            param.SuperIdsTransferTotal,
            param.ToCustIds,
            param.IsFullTransfer,
            param.Amount,
            param.CustStatus,
            param.AutoTransfer,
            param.Language,
            param.IP,
            param.CustId,
            param.SubAccId);
    }
}
