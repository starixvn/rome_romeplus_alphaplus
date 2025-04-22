using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class InsertPaymentMethodCriteria : NonQueryCommand
    {
        public InsertPaymentMethodCriteria(
            int providerId,
            string methodName,
            int transactionType,
            int creator)
        {
            ProviderId = providerId;
            TransactionType = transactionType;
            MethodName = methodName;
            Creator = creator;
        }

        [SpParam(SqlParamName = "ProviderId")]
        public int ProviderId { get; }

        [SpParam(SqlParamName = "MethodName")]
        public string MethodName { get; }

        [SpParam(SqlParamName = "MethodType")]
        public int TransactionType { get; }

        [SpParam(SqlParamName = "Creator")]
        public int Creator { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_InsMethod";

        public override bool IsValid() 
            => ProviderId > 0 && !string.IsNullOrWhiteSpace(MethodName) && TransactionType > 0 && Creator > 0;
    }
} 