using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetPaymentProviderSettingsCriteria : CriteriaBase
    {
        public GetPaymentProviderSettingsCriteria(int providerId, int method, int status, int transactionType)
        {
            ProviderId = providerId;
            Method = method;
            Status = status;
            TransactionType = transactionType;
        }

        public int ProviderId { get; set; }
        public int Method { get; set; }
        public int Status { get; set; }
        public int TransactionType { get; set; }

        public override string GetSettingKey() => "Age_ManagementPayments_SelProviderSettings";

        public override bool IsValid() => ProviderId > 0;
    }
} 