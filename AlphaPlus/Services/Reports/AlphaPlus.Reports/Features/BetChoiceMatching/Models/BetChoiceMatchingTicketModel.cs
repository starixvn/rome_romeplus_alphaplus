using System.Text.Json.Serialization;

namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public class BetChoiceMatchingTicketModel
{
    public int LeagueId { get; init; }

    [JsonIgnore]
    public int LeagueGroupId { get; init; }

    public DateTime GlobalShowTime { get; init; }

    public int SportType { get; init; }

    public long MatchId { get; init; }

    public int CustId { get; init; }

    public string Username { get; init; }

    public int BetType { get; init; }

    public string BetTeam { get; init; }

    public decimal Hdp1 { get; init; }

    public decimal Hdp2 { get; init; }

    public string IP { get; set; }

    public decimal Odds { get; set; }

    public string DisplayOdds { get; set; }

    [JsonIgnore]
    public byte OddsType { get; init; }

    public string DisplayOddsType { get; set; } = string.Empty;

    public DateTime TransDate { get; init; }

    [JsonIgnore]
    public long TransId { get; init; }

    public string TransIdString => TransId.ToString();

    public decimal Stake { get; init; }

    public decimal MemberWinLoss { get; init; }

    public decimal SuperWinLoss { get; init; }

    public decimal Company { get; init; }

    public int HomeId { get; init; }

    public int AwayId { get; init; }

    public bool IsLive { get; init; }

    public int LiveHomeScore { get; init; }

    public int LiveAwayScore { get; init; }

    public bool IsRunning { get; init; }

    [JsonIgnore]
    public string EventKey
        => $"{MatchId}_{BetType}_{Hdp1}_{Hdp2}_{BetTeam}_{IsLive}_{LiveHomeScore}_{LiveAwayScore}";
}
