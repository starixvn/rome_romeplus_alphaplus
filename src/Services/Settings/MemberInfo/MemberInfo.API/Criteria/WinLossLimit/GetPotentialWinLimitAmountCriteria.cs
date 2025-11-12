using Fanex.Data.Repository;

namespace AlphaPlus.Settings.MemberInfo.API.Criteria.WinLossLimit
{
    public class GetPotentialWinLimitAmountCriteria : CriteriaBase
    {
        public string CustIds { get; set; }

        public override string GetSettingKey()
            => "SiteDB_Age_CustomerSelPotentialWinlimitAmount";

        public override bool IsValid()
            => true;
    }
}

