using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

public class GetScoreMapResultMasterCriteria(
    int masterId,
    string matchIds,
    string betTypesLive,
    string betTypesDead) : CriteriaBase
{
    public int MRecommend { get; } = masterId;

    public string MatchIds { get; } = matchIds;

    public string BetTypes_Live { get; } = betTypesLive;

    public string BetTypes_Dead { get; } = betTypesDead;

    public override string GetSettingKey() => "Age_ScoreMap_Result_MAg";

    public override bool IsValid()
        => MRecommend > 0
        && !string.IsNullOrWhiteSpace(MatchIds)
        && (!string.IsNullOrWhiteSpace(BetTypes_Live) || !string.IsNullOrWhiteSpace(BetTypes_Dead));
}
