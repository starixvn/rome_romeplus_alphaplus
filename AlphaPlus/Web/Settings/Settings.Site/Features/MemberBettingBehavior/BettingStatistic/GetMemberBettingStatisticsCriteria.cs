using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public class GetMemberBettingStatisticsCriteria : CriteriaBase
    {
        public GetMemberBettingStatisticsCriteria(int custId, DateTime dateStart, DateTime dateEnd)
        {
            CustId = custId;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public int CustId { get; }

        public DateTime DateStart { get; }

        public DateTime DateEnd { get; }

        public override string GetSettingKey()
            => "Age_Rpt_CustomerBettingBehavior_Statistic_Sel";

        public override bool IsValid()
            => CustId > 0;
    }
}