using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetAllPaymentProvidersCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_ManagementPayments_SelAllProviders";

        public override bool IsValid() => true;
    }
} 