using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal abstract class GetScoreMapResultBaseCriteria(
    string matchIds,
    string liveBetTypes,
    string deadBetTypes) : CriteriaBase
{
    public string MatchIds { get; } = matchIds;

    public string BetTypes_Live { get; } = liveBetTypes;

    public string BetTypes_Dead { get; } = deadBetTypes;

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(MatchIds)
        && (!string.IsNullOrWhiteSpace(BetTypes_Live) || !string.IsNullOrWhiteSpace(BetTypes_Dead));
}