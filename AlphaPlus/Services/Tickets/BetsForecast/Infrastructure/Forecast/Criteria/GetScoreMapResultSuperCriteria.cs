using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

public class GetScoreMapResultSuperCriteria(
    int superId,
    string matchIds,
    string liveBetTypes,
    string deadBetTypes) : CriteriaBase
{
    public int SRecommend { get; } = superId;

    public string MatchIds { get; } = matchIds;

    public string BetTypes_Live { get; } = liveBetTypes;

    public string BetTypes_Dead { get; } = deadBetTypes;

    public override string GetSettingKey() => "Age_ScoreMap_Result_SAg";

    public override bool IsValid()
        => SRecommend > 0
        && !string.IsNullOrWhiteSpace(MatchIds)
        && (!string.IsNullOrWhiteSpace(BetTypes_Live) || !string.IsNullOrWhiteSpace(BetTypes_Dead));
}
