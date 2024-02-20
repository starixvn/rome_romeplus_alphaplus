using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Criteria;

public class GetScoreMapBetTypesCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_ScoreMap_SelBettype";

    public override bool IsValid() => true;
}