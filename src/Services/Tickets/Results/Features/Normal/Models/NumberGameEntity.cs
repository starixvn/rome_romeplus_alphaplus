namespace AlphaPlus.Results.Features.Normal;

public class NumberGameEntity
{
    public int LeagueId { get; init; }

    public int MatchId { get; init; }

    public string MatchCode { get; init; }

    public DateTime EventDate { get; init; }

    public DateTime KickOffTime { get; init; }

    public string EventStatus { get; init; }

    public int Seg { get; init; }

    public int Number { get; init; }
}