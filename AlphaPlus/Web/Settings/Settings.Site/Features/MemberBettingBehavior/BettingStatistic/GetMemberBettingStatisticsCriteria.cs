using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class GetMemberBettingStatisticsCriteria(int custId, DateTime dateStart, DateTime dateEnd) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public DateTime DateStart { get; } = dateStart;

        public DateTime DateEnd { get; } = dateEnd;

        public override string GetSettingKey()
            => "Age_Rpt_CustomerBettingBehavior_Statistic_Sel";

        public override bool IsValid()
            => CustId > 0;
    }
}