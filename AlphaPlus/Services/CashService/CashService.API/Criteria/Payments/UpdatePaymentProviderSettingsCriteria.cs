using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class UpdatePaymentProviderSettingsCriteria : NonQueryCommand
    {
        public UpdatePaymentProviderSettingsCriteria(
            int providerId,
            int method,
            int status,
            int transactionType,
            decimal minAmount,
            decimal maxAmount,
            bool isEnabled,
            bool bankRequired)
        {
            ProviderId = providerId;
            Method = method;
            Status = status;
            TransactionType = transactionType;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            IsEnabled = isEnabled;
            BankRequired = bankRequired;
        }

        [SpParam(SqlParamName = "ProviderId")]
        public int ProviderId { get; }

        [SpParam(SqlParamName = "Method")]
        public int Method { get; }

        [SpParam(SqlParamName = "Status")]
        public int Status { get; }

        [SpParam(SqlParamName = "TransactionType")]
        public int TransactionType { get; }

        [SpParam(SqlParamName = "MinAmount")]
        public decimal MinAmount { get; }

        [SpParam(SqlParamName = "MaxAmount")]
        public decimal MaxAmount { get; }

        [SpParam(SqlParamName = "IsEnabled")]
        public bool IsEnabled { get; }

        [SpParam(SqlParamName = "BankRequired")]
        public bool BankRequired { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_UpdProviderSettings";

        public override bool IsValid() => ProviderId > 0 && MinAmount >= 0 && MaxAmount >= MinAmount;
    }
} 