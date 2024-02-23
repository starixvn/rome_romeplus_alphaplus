using AlphaPlus.Core.Models;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo
{
    public class GetBalanceInfoCriteria(UserLevel userLevel, int custId, DateTime moveBetsDay) : CriteriaBase
    {
        private static readonly Dictionary<UserLevel, string> SettingKeys = new()
        {
            [UserLevel.Agent] = "Age_CustomerSelBalanceInfo_Ag",
            [UserLevel.Master] = "Age_CustomerSelBalanceInfo_MAg",
            [UserLevel.Super] = "Age_CustomerSelBalanceInfo_SAg"
        };

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; } = userLevel;

        public int CustId { get; } = custId;

        public DateTime MoveBetsDay { get; } = moveBetsDay;

        public override string GetSettingKey() => SettingKeys[UserLevel];

        public override bool IsValid() => CustId > 0;
    }
}