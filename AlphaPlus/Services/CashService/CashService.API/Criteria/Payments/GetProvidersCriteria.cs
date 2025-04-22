using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetProvidersCriteria : CriteriaBase
    {
        public GetProvidersCriteria(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; set; }

        public override string GetSettingKey() => "Age_CashService_PaymentProvider_Sel";

        public override bool IsValid() => CustId > 0;
    }
} 