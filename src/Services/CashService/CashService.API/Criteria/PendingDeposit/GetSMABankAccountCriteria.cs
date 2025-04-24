using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.PendingDeposit
{
    public class GetSMABankAccountCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_SMABankAcc";

        public override bool IsValid() => true;
    }
} 