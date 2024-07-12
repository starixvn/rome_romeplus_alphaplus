using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Criteria.MultipleTransfer;
using AlphaPlus.Settings.Transfer.API.Models.MultipleTransfer;
using AlphaPlus.Settings.Transfer.API.Params.MultipleTransfer;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Services;

public interface IMultipleTransferService
{
    Task<CustomerTransferReport> GetTransferList(GetTransferListParam param);
}

public class MultipleTransferService(IDynamicRepository repository) : IMultipleTransferService
{
    private readonly IDynamicRepository repository = repository;

    public async Task<CustomerTransferReport> GetTransferList(GetTransferListParam param)
    {
        var criteria = BuildCriteria(param);
        var customers = await repository.FetchAsync<CustomerTransfer>(criteria);

        return new CustomerTransferReport(customers, BuildTotal(criteria), criteria.TotalPages);
    }

    private static GetTransferListCriteria BuildCriteria(GetTransferListParam param)
    {
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevelId);

        if (custLevel.IsAgentLevel())
        {
            return new GetAgentTransferListCriteria(
                param.CustId,
                param.DonwlineName,
                param.Status,
                param.AllYesterdayBalance,
                param.AllowTotalBalanceTransfer,
                param.AutoTransfer,
                param.PageIndex,
                param.PageSize);
        }

        if (custLevel.IsMasterLevel())
        {
            return new GetMasterTransferListCriteria(
                param.CustId,
                param.DonwlineName,
                param.Status,
                param.AllYesterdayBalance,
                param.AllowTotalBalanceTransfer,
                param.AutoTransfer,
                param.PageIndex,
                param.PageSize);
        }

        return new GetSuperTransferListCriteria(
            param.CustId,
            param.DonwlineName,
            param.Status,
            param.AllYesterdayBalance,
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
                YesterdayBalance = agentCriteria.TotalYesterdayBalance,
                Balance = agentCriteria.TotalBalance,
                Outstanding = agentCriteria.TotalOutStanding,
                GivenCredit = agentCriteria.TotalCredit
            };
        }

        if (criteria is GetMasterTransferListCriteria masterCriteria)
        {
            return new CustomerTransfer
            {
                YesterdayBalance = masterCriteria.TotalYesterdayAgentBalance,
                Balance = masterCriteria.TotalAgentBalance,
                YesterdayCashBalance = masterCriteria.TotalYesterday_AgentCashBalance,
                CashBalance = masterCriteria.TotalAgentCashBalance,
                Outstanding = masterCriteria.TotalOutStanding,
                GivenCredit = masterCriteria.TotalCredit,
                UsedCredit = masterCriteria.TotalUsedCredit
            };
        }

        if (criteria is GetSuperTransferListCriteria superCriteria)
        {
            return new CustomerTransfer
            {
                YesterdayBalance = superCriteria.TotalYesterdayMAgentBalance,
                Balance = superCriteria.TotalMAgentBalance,
                YesterdayCashBalance = superCriteria.TotalYesterday_MAgentCashBalance,
                CashBalance = superCriteria.TotalMAgentCashBalance,
                Outstanding = superCriteria.TotalOutStanding,
                GivenCredit = superCriteria.TotalCredit,
                UsedCredit = superCriteria.TotalUsedCredit
            };
        }

        return new CustomerTransfer();
    }
}
