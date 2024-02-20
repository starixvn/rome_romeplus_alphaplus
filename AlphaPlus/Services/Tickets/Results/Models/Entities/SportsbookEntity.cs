namespace AlphaPlus.Results.Models.Entities;

public class SportsbookEntity
{
    public int SportType { get; init; }
    public int Handicap2 { get; init; }
    public string BetCheck { get; init; }
    public int Bettype { get; init; }
    public string RefNo { get; init; }
    public bool IsHTCal { get; init; }
    public bool Calculate { get; init; }
    public string GoalRecord { get; init; }
    public bool IsNeutral { get; init; }
    public string EventStatus { get; init; }
    public int? LiveAwayScore { get; init; }
    public byte SettlementStatus { get; init; }
    public int? LiveHomeScore { get; init; }
    public int? FinalHomeScore { get; init; }
    public int? HtAwayScore { get; init; }
    public int? HtHomeScore { get; init; }
    public int AwayId { get; init; }
    public int HomeId { get; init; }
    public DateTime KickOffTime { get; init; }
    public DateTime EventDate { get; init; }
    public string MatchCode { get; init; }
    public string Lcolor { get; init; }
    public int MatchId { get; init; }
    public int LeagueId { get; init; }
    public string BetStatus { get; init; }
    public int? FinalAwayScore { get; init; }
    public bool NoHTScore { get; init; }
}