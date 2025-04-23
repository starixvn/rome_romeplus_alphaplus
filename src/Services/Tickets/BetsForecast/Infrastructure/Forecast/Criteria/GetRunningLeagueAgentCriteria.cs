using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetRunningLeagueAgentCriteria(int agentId, string betTypes) : CriteriaBase
{
    public int AgId { get; } = agentId;

    public string BetTypes { get; } = betTypes;

    public override string GetSettingKey()
        => "Age_LeagueSelRunning_Ag";

    public override bool IsValid()
        => AgId > 0 && !string.IsNullOrWhiteSpace(BetTypes);
}
