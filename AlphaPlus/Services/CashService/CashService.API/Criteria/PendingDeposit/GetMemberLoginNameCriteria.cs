using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit
{
    public class GetMemberLoginNameCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_ManualDepMember";

        public override bool IsValid() => true;
    }
} 