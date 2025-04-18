using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.ManagePayments
{
    public class GetPaymentProvidersFeesCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_ManagementPayments_SelProvidersFees";

        public override bool IsValid() => true;
    }
} 