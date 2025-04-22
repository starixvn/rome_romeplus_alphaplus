using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetAllBanksCriteria : CriteriaBase
    {
        public GetAllBanksCriteria()
        {
        }

        public override string GetSettingKey() => "Age_ManagementPayments_SelAllBanks";

        public override bool IsValid() => true;
    }
} 