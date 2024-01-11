using AlphaPlus.BetsForecast.Domain.Forecast;

namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class TotalBetsMatch : Match
{
    public string ShowTime { get; init; }

    public int LeagueId { get; init; }

    public string LeagueName { get; set; }

    public bool LiveIndicator { get; init; }

    public bool IsNeutral { get; init; }

    public new string Event => $"{HomeName} -vs- {AwayName}";

    public int LiveHomeScore { get; init; }

    public int LiveAwayScore { get; init; }
}
