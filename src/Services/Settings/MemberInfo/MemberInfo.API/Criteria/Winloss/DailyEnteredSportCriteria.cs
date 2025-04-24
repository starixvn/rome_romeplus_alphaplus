using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLoss
{
    public class DailyEnteredSportCriteria : CriteriaBase
    {
        public DailyEnteredSportCriteria(bool includeTestData)
        {
            this.IncludeTestData = includeTestData;
        }

        public bool IncludeTestData { get; }

        public override string GetSettingKey() => "Age_Rpt_MatchSelResultEntered";

        public override bool IsValid() => true;
    }
}