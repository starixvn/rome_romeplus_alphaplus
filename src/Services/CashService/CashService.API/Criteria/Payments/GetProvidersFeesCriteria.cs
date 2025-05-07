using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetProvidersFeesCriteria(int providerId) : CriteriaBase
    {
        public int ProviderId { get; set; } = providerId;

        public override string GetSettingKey() => "Age_CashService_PaymentProviderFee_Sel";

        public override bool IsValid() => ProviderId > 0;
    }
} 