using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

public class GetScoreMapResultAgentCriteria(
    int agentId,
    string matchIds,
    string betTypesLive,
    string betTypesDead) : CriteriaBase
{
    public int Recommend { get; } = agentId;

    public string MatchIds { get; } = matchIds;

    public string BetTypes_Live { get; } = betTypesLive;

    public string BetTypes_Dead { get; } = betTypesDead;

    public override string GetSettingKey() => "Age_ScoreMap_Result_Ag";

    public override bool IsValid()
        => Recommend > 0
        && !string.IsNullOrWhiteSpace(MatchIds)
        && (!string.IsNullOrWhiteSpace(BetTypes_Live) || !string.IsNullOrWhiteSpace(BetTypes_Dead));
}
