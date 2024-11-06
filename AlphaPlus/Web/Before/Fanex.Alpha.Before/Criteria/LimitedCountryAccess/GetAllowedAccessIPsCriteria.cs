using Fanex.Data.Repository;

namespace Fanex.Alpha.Before.Criteria.LimitedCountryAccess
{
    public class GetAllowedAccessIPsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Age_AllowedAccessIPsSel";

        public override bool IsValid() => true;
    }
}
