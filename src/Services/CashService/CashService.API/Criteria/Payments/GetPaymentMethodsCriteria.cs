using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetPaymentMethodsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_PaymentMethods";

        public override bool IsValid() => true;
    }
} 