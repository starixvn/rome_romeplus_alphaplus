using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetScoreMapLeagueCriteria(int custId) : CriteriaBase
{
    public int AgentId { get; } = custId;

    public override string GetSettingKey() => "Age_ScoreMap_SelLeague";

    public override bool IsValid() => AgentId > 0;
}
