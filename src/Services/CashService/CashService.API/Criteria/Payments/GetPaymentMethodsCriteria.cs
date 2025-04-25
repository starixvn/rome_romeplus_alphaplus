using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetPaymentMethodsCriteria : CriteriaBase
    {
        public GetPaymentMethodsCriteria( int custId, int providerId, int transactionType)
        {
            CustId = custId;
            ProviderId = providerId;
            TransactionType = transactionType == 0 ? null : transactionType;
        }

        public int CustId { get; set; }
        public int ProviderId { get; set; }
        public int? TransactionType { get; set; }

        public override string GetSettingKey() => "Age_CashService_PaymentMethods";

        public override bool IsValid() => ProviderId >= 0 && CustId >= 0;
    }
} 