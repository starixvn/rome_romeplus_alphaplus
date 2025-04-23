using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Shared.Criteria;

internal class GetBalanceUpDayAndMoveBetsDayCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_SelBalanceUpDay";

    public override bool IsValid() => true;
}