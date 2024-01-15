using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetSoccerMatchCriteria(int leagueId, bool isLive) : CriteriaBase
{
    public int LeagueId { get; } = leagueId;

    public bool IsLive { get; } = isLive;

    public override string GetSettingKey() => "Age_NewTotalBetsForeCast_EventSel";

    public override bool IsValid() => LeagueId > 0;
}