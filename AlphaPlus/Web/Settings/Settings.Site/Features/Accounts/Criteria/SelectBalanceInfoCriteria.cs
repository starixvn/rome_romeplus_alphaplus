using AlphaPlus.Core.Models;
using Fanex.Data;

namespace AlphaPlus.Settings.Site.Features.Accounts.Criteria
{
    public class SelectBalanceInfoCriteria : AccountInfoCriteriaBase
    {
        private readonly IDictionary<UserLevel, string> settingKeys =
            new Dictionary<UserLevel, string>
            {
                {UserLevel.Master, "Age_BalanceMasterSel" },
                {UserLevel.Agent, "Age_BalanceAgentSel" },
                {UserLevel.Member, "Age_BalanceMemberSel" }
            };

        public SelectBalanceInfoCriteria(int searchAccountId, byte searchAccountRoleId)
            : base(searchAccountId)
        {
            SearchAccountRoleId = searchAccountRoleId;
        }

        [SpParam(Ignored = true)]
        public byte SearchAccountRoleId { get; }

        public override string GetSettingKey() => settingKeys[Enumeration.FromValue<UserLevel>(SearchAccountRoleId)];
    }
}