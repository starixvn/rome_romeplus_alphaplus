using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetProvidersFeesCriteria : CriteriaBase
    {
        public GetProvidersFeesCriteria(int providerId)
        {
            ProviderId = providerId;
        }

        public int ProviderId { get; set; }

        public override string GetSettingKey() => "Age_CashService_PaymentProviderFee_Sel";

        public override bool IsValid() => ProviderId > 0;
    }
} 