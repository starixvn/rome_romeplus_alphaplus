using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class CheckShowBetChoiceLimitCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; } = custId;

        public override string GetSettingKey() => "Age_CustSetting_BetChoiceLimitCheck";

        public override bool IsValid() => CustId > 0;
    }
}