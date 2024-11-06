using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Dashboard.BalanceInfo
{
    public class GetBalanceUpDayInfoCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_SelBalanceUpDay";

        public override bool IsValid() => true;
    }
}