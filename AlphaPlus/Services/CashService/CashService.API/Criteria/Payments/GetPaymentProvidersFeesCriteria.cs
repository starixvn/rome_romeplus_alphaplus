using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetPaymentProvidersFeesCriteria : CriteriaBase
    {
        public GetPaymentProvidersFeesCriteria(int providerId)
        {
            ProviderId = providerId;
        }

        public int ProviderId { get; set; }

        public override string GetSettingKey() => "Age_ManagementPayments_SelProvidersFees";

        public override bool IsValid() => ProviderId > 0;
    }
} 