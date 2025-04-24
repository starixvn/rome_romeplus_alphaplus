using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetProvidersFeesCriteria : CriteriaBase
    {
        public GetProvidersFeesCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; set; }

        public override string GetSettingKey() => "Age_CashService_PaymentProviderFee_Sel";

        public override bool IsValid() => CustId > 0;
    }
} 