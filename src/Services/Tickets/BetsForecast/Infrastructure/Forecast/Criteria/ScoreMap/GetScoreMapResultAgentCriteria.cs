namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetScoreMapResultAgentCriteria(
    int agentId,
    string matchIds,
    string liveBetTypes,
    string deadBetTypes) : GetScoreMapResultBaseCriteria(matchIds, liveBetTypes, deadBetTypes)
{
    public int Recommend { get; } = agentId;

    public override string GetSettingKey() => "Age_ScoreMap_Result_Ag";

    public override bool IsValid() => Recommend > 0 && base.IsValid();
}
