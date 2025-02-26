namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class League
{
    public int LeagueId { get; init; }

    public int LeagueGroupId { get; init; }

    public string Id => $"{LeagueId}_{LeagueType}";

    public string Name { get; set; }

    public int SportTypeId { get; init; }

    public byte LeagueType { get; init; }
}
