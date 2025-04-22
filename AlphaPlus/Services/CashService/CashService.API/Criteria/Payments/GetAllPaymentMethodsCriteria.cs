using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetAllPaymentMethodsCriteria : CriteriaBase
    {
        public GetAllPaymentMethodsCriteria(int providerId)
        {
            ProviderId = providerId;
        }

        [SpParam(SqlParamName = "ProviderId")]
        public int ProviderId { get; }

        public override string GetSettingKey() => "Age_ManagementPayments_SelAllMethods";

        public override bool IsValid() => ProviderId > 0;
    }
} 