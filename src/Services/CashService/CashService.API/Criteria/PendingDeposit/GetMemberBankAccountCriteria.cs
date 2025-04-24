using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit
{
    public class GetMemberBankAccountCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_MemberBankAcc";

        public override bool IsValid() => true;
    }
} 