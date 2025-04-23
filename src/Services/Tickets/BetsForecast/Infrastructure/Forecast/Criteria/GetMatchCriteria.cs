using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;

internal class GetMatchCriteria(int matchId, int betType) : CriteriaBase
{
    public int MatchId { get; } = matchId;

    public int BetType { get; } = betType;

    public override string GetSettingKey() => "Age_Forecast_OddsSelTopByMatch";

    public override bool IsValid() => MatchId > 0 && BetType > 0;
}
