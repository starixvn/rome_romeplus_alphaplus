using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.Forecast;

internal class GetScoreMapBetTypesCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_ScoreMap_SelBettype";

    public override bool IsValid() => true;
}
