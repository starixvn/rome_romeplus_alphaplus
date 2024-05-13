using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Shared;

public class GetBalanceUpAndMoveBetsDayCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_SelBalanceUpDay";

    public override bool IsValid() => true;
}