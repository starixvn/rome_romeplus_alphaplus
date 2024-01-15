namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetScoreMapResultMasterCriteria(
    int masterId,
    string matchIds,
    string liveBetTypes,
    string deadBetTypes) : GetScoreMapResultBaseCriteria(matchIds, liveBetTypes, deadBetTypes)
{
    public int MRecommend { get; } = masterId;

    public override string GetSettingKey() => "Age_ScoreMap_Result_MAg";

    public override bool IsValid() => MRecommend > 0 && base.IsValid();
}
