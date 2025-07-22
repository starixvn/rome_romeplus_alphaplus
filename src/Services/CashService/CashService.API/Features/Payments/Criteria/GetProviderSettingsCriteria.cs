using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetProviderSettingsCriteria(int providerId, int custId, int method, int status, int transactionType)
        : CriteriaBase
    {
        public int ProviderId { get; } = providerId;
        public int CustId { get; } = custId;

        [SpParam(SqlParamName = "PaymentMethodID")]
        public int Method { get; } = method;

        [SpParam(SqlParamName = "PaymentMethodStatus")]
        public int Status { get; } = status;

        [SpParam(SqlParamName = "TransactionType")]
        public int TransactionType { get; } = transactionType;

        public override string GetSettingKey() => "Age_CashService_PaymentProviderDetail_Sel";

        public override bool IsValid() => ProviderId > 0;
    }
} 