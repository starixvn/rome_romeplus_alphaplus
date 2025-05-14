using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetPaymentMethodsCriteria(int custId, int providerId, int transactionType) : CriteriaBase
    {
        public int CustId { get; set; } = custId;
        public int ProviderId { get; set; } = providerId;
        public int? TransactionType { get; set; } = transactionType == 0 ? null : transactionType;

        public override string GetSettingKey() => "Age_CashService_PaymentMethods";

        public override bool IsValid() => ProviderId >= 0 && CustId >= 0;
    }
} 