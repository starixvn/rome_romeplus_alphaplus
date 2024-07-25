using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.TransferList;
using AlphaPlus.Settings.Transfer.API.Models.TransferList;
using AlphaPlus.Settings.Transfer.API.Params.TransferList;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface ITransferListService
{
    Task<CustomerTransferReport> GetTransferList(TransferListParam param);
}

public class TransferListService(IDynamicRepository repository) : ITransferListService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<CustomerTransferReport> GetTransferList(TransferListParam param)
    {
        var criteria = BuildTransferListCriteria(param);
        var customers = await repository.FetchAsync<CustomerTransfer>(criteria, param.AGIP);

        return new CustomerTransferReport(customers, BuildTotal(criteria), criteria.TotalPages);
    }

    private static GetTransferListCriteria BuildTransferListCriteria(TransferListParam param)
    {
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevelId);

        if (custLevel.IsAgentLevel())
        {
            return new GetAgentTransferListCriteria(
                param.CustId,
                param.DownlineName,
                param.Status,
                !param.AllYesterdayBalance,
                param.AllowTotalBalanceTransfer,
                param.AutoTransfer,
                param.PageIndex,
                param.PageSize);
        }

        if (custLevel.IsMasterLevel())
        {
            return new GetMasterTransferListCriteria(
                param.CustId,
                param.DownlineName,
                param.Status,
                !param.AllYesterdayBalance,
                param.AllowTotalBalanceTransfer,
                param.AutoTransfer,
                param.PageIndex,
                param.PageSize);
        }

        return new GetSuperTransferListCriteria(
            param.CustId,
            param.DownlineName,
            param.Status,
            !param.AllYesterdayBalance,
            param.AllowTotalBalanceTransfer,
            param.AutoTransfer,
            param.PageIndex,
            param.PageSize);
    }

    private static CustomerTransfer BuildTotal(GetTransferListCriteria criteria)
    {
        if (criteria is GetAgentTransferListCriteria agentCriteria)
        {
            return new CustomerTransfer
            {
                YesterdayBalance = agentCriteria.TotalYesterdayBalance ?? 0,
                Balance = agentCriteria.TotalBalance ?? 0,
                Outstanding = agentCriteria.TotalOutStanding ?? 0,
                GivenCredit = agentCriteria.TotalCredit ?? 0
            };
        }

        if (criteria is GetMasterTransferListCriteria masterCriteria)
        {
            return new CustomerTransfer
            {
                YesterdayBalance = masterCriteria.TotalYesterdayAgentBalance ?? 0,
                Balance = masterCriteria.TotalAgentBalance ?? 0,
                YesterdayCashBalance = masterCriteria.TotalYesterday_AgentCashBalance ?? 0,
                CashBalance = masterCriteria.TotalAgentCashBalance ?? 0,
                Outstanding = masterCriteria.TotalOutStanding ?? 0,
                GivenCredit = masterCriteria.TotalCredit ?? 0,
                UsedCredit = masterCriteria.TotalUsedCredit ?? 0
            };
        }

        if (criteria is GetSuperTransferListCriteria superCriteria)
        {
            return new CustomerTransfer
            {
                YesterdayBalance = superCriteria.TotalYesterdayMAgentBalance ?? 0,
                Balance = superCriteria.TotalMAgentBalance ?? 0,
                YesterdayCashBalance = superCriteria.TotalYesterday_MAgentCashBalance ?? 0,
                CashBalance = superCriteria.TotalMAgentCashBalance ?? 0,
                Outstanding = superCriteria.TotalOutStanding ?? 0,
                GivenCredit = superCriteria.TotalCredit ?? 0,
                UsedCredit = superCriteria.TotalUsedCredit ?? 0
            };
        }

        return new CustomerTransfer();
    }
}