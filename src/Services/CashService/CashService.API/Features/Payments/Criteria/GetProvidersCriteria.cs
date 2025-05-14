using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetProvidersCriteria(int custId) : CriteriaBase
    {
        public int CustId { get; set; } = custId;

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Sel";

        public override bool IsValid() => CustId > 0;
    }
} 