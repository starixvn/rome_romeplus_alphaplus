using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class GetBalanceUpDayAndMoveBetsDayCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_SelBalanceUpDay";

    public override bool IsValid() => true;
}