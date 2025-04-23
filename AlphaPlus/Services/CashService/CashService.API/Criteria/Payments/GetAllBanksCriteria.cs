using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetAllBanksCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_BankList";

        public override bool IsValid() => true;
    }
} 