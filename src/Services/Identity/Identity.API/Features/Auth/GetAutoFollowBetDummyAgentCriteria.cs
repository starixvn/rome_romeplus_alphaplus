using Fanex.Data.Repository;

namespace AlphaPlus.Identity.API.Features.Auth
{
    public class GetAutoFollowBetDummyAgentCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "NAP_Customer_FollowBetAccountDummyAgentSel";

        public override bool IsValid() => true;
    }
}