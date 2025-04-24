namespace AlphaPlus.BetLists.LastBets;

public class MatchInfo
{
    public int MatchId { get; init; }

    public string MatchCode { get; init; }

    public int SportType { get; init; }

    public int LeagueId { get; init; }

    public int LeagueGroupId { get; init; }

    public int HomeId { get; init; }

    public int AwayId { get; init; }

    public string Race { get; init; }

    public string ShowTime { get; init; }

    public string Status { get; init; }

    public bool IsNeutral { get; init; }

    public string BetStatus { get; init; }

    public byte SettlementStatus { get; init; }

    public DateTime EventDate { get; init; }
}