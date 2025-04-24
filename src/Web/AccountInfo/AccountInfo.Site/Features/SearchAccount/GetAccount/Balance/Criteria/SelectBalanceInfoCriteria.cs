using AlphaPlus.Core.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria
{
    public class SelectBalanceInfoCriteria(int custId, byte roleId) : CriteriaBase
    {
        private readonly IDictionary<UserLevel, string> settingKeys =
            new Dictionary<UserLevel, string>
            {
                {UserLevel.Master, "Age_BalanceMasterSel" },
                {UserLevel.Agent, "Age_BalanceAgentSel" },
                {UserLevel.Member, "Age_BalanceMemberSel" }
            };

        public int CustId { get; } = custId;

        [SpParam(Ignored = true)]
        public byte RoleId { get; } = roleId;

        public override string GetSettingKey() => settingKeys[Enumeration.FromValue<UserLevel>(RoleId)];

        public override bool IsValid() => CustId > 0;
    }
}