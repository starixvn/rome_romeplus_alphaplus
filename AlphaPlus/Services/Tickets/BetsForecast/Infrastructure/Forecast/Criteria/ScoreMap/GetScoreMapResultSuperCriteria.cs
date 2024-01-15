namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetScoreMapResultSuperCriteria(
    int superId,
    string matchIds,
    string liveBetTypes,
    string deadBetTypes) : GetScoreMapResultBaseCriteria(matchIds, liveBetTypes, deadBetTypes)
{
    public int SRecommend { get; } = superId;

    public override string GetSettingKey() => "Age_ScoreMap_Result_SAg";

    public override bool IsValid() => SRecommend > 0 && base.IsValid();
}
