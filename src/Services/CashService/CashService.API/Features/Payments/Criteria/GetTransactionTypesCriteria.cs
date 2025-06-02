using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Payments.Criteria
{
    public class GetTransactionTypesCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CashService_TransTypeControl";

        public override bool IsValid() => true;
    }
}