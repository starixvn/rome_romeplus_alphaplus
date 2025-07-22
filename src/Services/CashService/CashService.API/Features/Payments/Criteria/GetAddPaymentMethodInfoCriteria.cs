using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetAddPaymentMethodInfoCriteria(int custId, int providerId, int transactionType) : CriteriaBase
    {
        public int CustId { get; } = custId;
        public int ProviderId { get; } = providerId;
        public int? TransactionType { get; } = transactionType == 0 ? null : transactionType;

        public string ChannelContact { get; set; }

        public override string GetSettingKey() => "Age_CashService_PaymentMethods";

        public override bool IsValid() => ProviderId >= 0 && CustId >= 0;
    }
} 