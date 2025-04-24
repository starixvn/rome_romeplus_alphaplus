using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetProviderSettingsCriteria : CriteriaBase
    {
        public GetProviderSettingsCriteria(int providerId, int method, int status, int transactionType)
        {
            ProviderId = providerId;
            Method = method;
            Status = status;
            TransactionType = transactionType;
        }

        public int ProviderId { get; set; }

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; set; }

        [SpParam(SqlParamName = "PaymentMethodStatus")]
        public int Status { get; set; }

        [SpParam(SqlParamName = "TransactionType")]
        public int TransactionType { get; set; }

        public override string GetSettingKey() => "Age_CashService_PaymentProviderDtl_Sel";

        public override bool IsValid() => ProviderId > 0;
    }
} 