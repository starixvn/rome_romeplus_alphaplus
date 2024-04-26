namespace AlphaPlus.Settings.MemberInfo.API.Services.CreditBalance
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Criteria.CreditBalance;
    using Fanex.Data.Repository;
    using AlphaPlus.Utilities.IP;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance;
    using AlphaPlus.Settings.MemberInfo.Domain.Params;

    public interface CreditBalanceService
    {
        Task<PagedCreditBalance> GetPagedCreditBalancesAsync(CreditBalanceParam param);

        Task<PagedCreditBalance> GetCreditBalancesAsync(CreditBalanceParam param);

        Task<IDictionary<int, CustomerClosedReason>> GetClosedReasonsAsync(CreditBalanceParam param);

        Task<IDictionary<int, decimal>> GetAgentNetPositionsAsync(CreditBalanceParam param);
    }

    public class CreditBalanceServiceImpl : CreditBalanceService
    {
        private readonly IDynamicRepository repository;
        private readonly IPService ipService;

        public CreditBalanceServiceImpl(
            IDynamicRepository dynamicRepository,
            IPService ipService)
        {
            repository = dynamicRepository;
            this.ipService = ipService;
        }

        public async Task<PagedCreditBalance> GetPagedCreditBalancesAsync(CreditBalanceParam param)
        {
            var closedReasons = await GetClosedReasonsAsync(param);
            var agentNetPositions = await GetAgentNetPositionsAsync(param);
            var pagedCreditBalance = await GetCreditBalancesAsync(param);

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

                if (agentNetPositions.ContainsKey(item.CustId))
                {
                    item.BetCredit += agentNetPositions[item.CustId];
                }
            }

            return pagedCreditBalance;
        }

        public async Task<PagedCreditBalance> GetCreditBalancesAsync(CreditBalanceParam param)
        {
            GetPagedCreditBalanceCriteria? criteria = null;

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
                    Enumerable.Empty<CreditBalanceItem>());
            }

            var creditBalances = await repository.FetchAsync<CreditBalanceItem>(criteria);

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
                param.PageIndex,
                param.PageSize);

        private static GetPagedCreditBalanceCriteria GetMasterCreditBalanceCriteria(CreditBalanceParam param)
            => new GetMasterCreditBalanceCriteria(
                param.CustId,
                param.UserName,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.PageIndex,
                param.PageSize);

        private static GetPagedCreditBalanceCriteria GetAgentCreditBalanceCriteria(CreditBalanceParam param)
            => new GetAgentCreditBalanceCriteria(
                param.CustId,
                param.UserName,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.PageIndex,
                param.PageSize);

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

            var closedReasons = await repository.FetchAsync<CustomerClosedReason>(criteria);

            return closedReasons.ToDictionary(kvp => kvp.CustId, kvp => kvp);
        }

        private static GetSuperClosedReasonCriteria GetSuperClosedReasonCriteria(CreditBalanceParam param)
            => new GetSuperClosedReasonCriteria(
                param.CustId,
                param.UserName,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.PageIndex,
                param.PageSize);

        private static GetMasterClosedReasonCriteria GetMasterClosedReasonCriteria(CreditBalanceParam param)
            => new GetMasterClosedReasonCriteria(
                param.CustId,
                param.UserName,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.PageIndex,
                param.PageSize);

        private static GetAgentClosedReasonCriteria GetAgentClosedReasonCriteria(CreditBalanceParam param)
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
            var agentNetPositions = await repository.FetchAsync<AgentNetPosition>(criteria, param.AGIP);

            return agentNetPositions.ToDictionary(kvp => kvp.CustId, kvp => kvp.NetPosition);
        }

        private static ICriteria GetAgentNetPositionCriteria(CreditBalanceParam param)
            => new GetAgentNetPositionCriteria(
                param.CustId,
                param.UserName,
                param.Enabled,
                param.Closed,
                param.Suspended,
                param.PageIndex,
                param.PageSize);
    }
}
