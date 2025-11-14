using AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;
using AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance;
using AlphaPlus.Settings.MemberInfo.Domain.Params;
using AlphaPlus.Utilities.IP;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Services.CreditBalance;

public interface CreditBalanceService
{
    Task<PagedCreditBalance> GetPagedCreditBalancesAsync(CreditBalanceParam param);

    Task<PagedCreditBalance> GetCreditBalancesAsync(CreditBalanceParam param);

    Task<IDictionary<int, CustomerClosedReason>> GetClosedReasonsAsync(CreditBalanceParam param);

    Task<IDictionary<int, decimal>> GetAgentNetPositionsAsync(CreditBalanceParam param);
}

public class CreditBalanceServiceImpl(IDynamicRepository dynamicRepository, IPService ipService) : CreditBalanceService
{
    public async Task<PagedCreditBalance> GetPagedCreditBalancesAsync(CreditBalanceParam param)
    {
        var closedReasonsTask = GetClosedReasonsAsync(param);
        var agentNetPositionsTask = GetAgentNetPositionsAsync(param);
        var pagedCreditBalanceTask = GetCreditBalancesAsync(param);
        await Task.WhenAll(closedReasonsTask, pagedCreditBalanceTask, pagedCreditBalanceTask);

        var closedReasons = await closedReasonsTask;
        var agentNetPositions = await agentNetPositionsTask;
        var pagedCreditBalance = await pagedCreditBalanceTask;

        foreach (var item in pagedCreditBalance.CreditBalances)
        {
            if (!param.IsAdminCanViewVNIP)
            {
                item.IPLogin = FormatIP(item.IPLogin ?? string.Empty);
            }

            if (closedReasons.TryGetValue(item.CustId, out var customerClosed))
            {
                item.ClosedReason = customerClosed.ClosedReason;
                item.Locked = customerClosed.Locked;
            }

            if (agentNetPositions.TryGetValue(item.CustId, out var position))
            {
                item.BetCredit += position;
            }
        }

        return pagedCreditBalance;
    }

    public async Task<PagedCreditBalance> GetCreditBalancesAsync(CreditBalanceParam param)
    {
        GetPagedCreditBalanceCriteria criteria;
        if (param.CustLevel.IsSuperLevel())
        {
            criteria = GetSuperCreditBalanceCriteria(param);
        }
        else if (param.CustLevel.IsMasterLevel())
        {
            criteria = GetMasterCreditBalanceCriteria(param);
        }
        else if (param.CustLevel.IsAgentLevel())
        {
            criteria = GetAgentCreditBalanceCriteria(param);
        }
        else
        {
            return new PagedCreditBalance(
                param.PageIndex,
                param.PageSize,
                0,
                []);
        }

        var creditBalances = await dynamicRepository.FetchAsync<CreditBalanceItem>(criteria);

        return new PagedCreditBalance(
            criteria.PageIndex,
            criteria.PageSize,
            criteria.TotalItems,
            creditBalances);
    }

    private string FormatIP(string ip)
        => !string.IsNullOrWhiteSpace(ip) && ip != "::1" && ipService.IsVNIP(ip)
            ? string.Empty
            : ip;

    private static GetPagedCreditBalanceCriteria GetSuperCreditBalanceCriteria(CreditBalanceParam param)
        => new GetSuperCreditBalanceCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.LevelType,
            param.PageIndex,
            param.PageSize)
        {
            SiteDbGroupId = param.SiteDbGroupId
        };

    private static GetPagedCreditBalanceCriteria GetMasterCreditBalanceCriteria(CreditBalanceParam param)
        => new GetMasterCreditBalanceCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.LevelType,
            param.PageIndex,
            param.PageSize)
        {
            SiteDbGroupId = param.SiteDbGroupId
        };

    private static GetPagedCreditBalanceCriteria GetAgentCreditBalanceCriteria(CreditBalanceParam param)
        => new GetAgentCreditBalanceCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.PageIndex,
            param.PageSize)
        {
            SiteDbGroupId = param.SiteDbGroupId
        };

    public async Task<IDictionary<int, CustomerClosedReason>> GetClosedReasonsAsync(CreditBalanceParam param)
    {
        ICriteria criteria;

        if (param.CustLevel.IsSuperLevel())
        {
            criteria = GetSuperClosedReasonCriteria(param);
        }
        else if (param.CustLevel.IsMasterLevel())
        {
            criteria = GetMasterClosedReasonCriteria(param);
        }
        else if (param.CustLevel.IsAgentLevel())
        {
            criteria = GetAgentClosedReasonCriteria(param);
        }
        else
        {
            return new Dictionary<int, CustomerClosedReason>();
        }

        var closedReasons = await dynamicRepository.FetchAsync<CustomerClosedReason>(criteria);

        return closedReasons.ToDictionary(kvp => kvp.CustId, kvp => kvp);
    }

    private static ICriteria GetSuperClosedReasonCriteria(CreditBalanceParam param)
        => new GetSuperClosedReasonCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.LevelType,
            param.PageIndex,
            param.PageSize);

    private static ICriteria GetMasterClosedReasonCriteria(CreditBalanceParam param)
        => new GetMasterClosedReasonCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.LevelType,
            param.PageIndex,
            param.PageSize);

    private static ICriteria GetAgentClosedReasonCriteria(CreditBalanceParam param)
        => new GetAgentClosedReasonCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.PageIndex,
            param.PageSize);

    public async Task<IDictionary<int, decimal>> GetAgentNetPositionsAsync(CreditBalanceParam param)
    {
        if (param.CustLevel.IsNotAgentLevel())
        {
            return new Dictionary<int, decimal>();
        }

        var criteria = GetAgentNetPositionCriteria(param);
        var agentNetPositions = await dynamicRepository.FetchAsync<AgentNetPosition>(criteria);

        return agentNetPositions.ToDictionary(kvp => kvp.CustId, kvp => kvp.NetPosition);
    }

    private static ICriteria GetAgentNetPositionCriteria(CreditBalanceParam param)
    {
        var criteria = new GetAgentNetPositionCriteria(
            param.CustId,
            param.UserName,
            param.Enabled,
            param.Closed,
            param.Suspended,
            param.PageIndex,
            param.PageSize)
        {
            SiteDbGroupId = param.SiteDbGroupId
        };
        return criteria;
    }
}