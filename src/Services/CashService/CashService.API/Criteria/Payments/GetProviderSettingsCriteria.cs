using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetProviderSettingsCriteria(int providerId, int method, int status, int transactionType)
        : CriteriaBase
    {
        public int ProviderId { get; set; } = providerId;

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; set; } = method;

        [SpParam(SqlParamName = "PaymentMethodStatus")]
        public int Status { get; set; } = status;

        [SpParam(SqlParamName = "TransactionType")]
        public int TransactionType { get; set; } = transactionType;

        public override string GetSettingKey() => "Age_CashService_PaymentProviderDtl_Sel";

        public override bool IsValid() => ProviderId > 0;
    }
} 