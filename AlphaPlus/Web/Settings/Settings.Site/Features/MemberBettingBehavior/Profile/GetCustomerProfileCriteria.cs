using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile
{
    public class GetCustomerProfileCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Age_CustomerBehaviorProfile_Sel";

        public override bool IsValid() => CustId > 0;
    }
}