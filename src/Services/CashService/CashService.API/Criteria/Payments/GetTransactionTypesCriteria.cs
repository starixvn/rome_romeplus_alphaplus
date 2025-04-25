using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Criteria.Payments
{
    public class GetTransactionTypesCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_TransTypeControl";

        public override bool IsValid() => true;
    }
} 