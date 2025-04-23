using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetScoreMapMatchCriteria(int custId, int leagueId) : CriteriaBase
{
    public int AgentId { get; } = custId;

    public int LeagueId { get; } = leagueId;

    public override string GetSettingKey() => "Age_ScoreMap_SelMatch";

    public override bool IsValid() => AgentId > 0 && LeagueId > 0;
}
