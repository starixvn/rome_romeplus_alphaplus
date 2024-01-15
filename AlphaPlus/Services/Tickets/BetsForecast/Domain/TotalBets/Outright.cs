namespace AlphaPlus.BetsForecast.Domain.TotalBets;

public class Outright
{
    public int LeagueId { get; init; }

    public string LeagueName { get; set; }

    public int OrId { get; init; }

    public int TeamId { get; init; }

    public string TeamName { get; set; }

    public decimal Balance { get; init; }

    public int BetCount { get; init; }

    public decimal TotalWinLost { get; init; }

    public bool IsNeutral { get; init; }
}
