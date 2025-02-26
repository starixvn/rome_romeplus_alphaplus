using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetScoreMapBetTypeCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_ScoreMap_SelBettype";

    public override bool IsValid() => true;
}
