namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class MatchWinLossDetailModel(
    MatchWinLossDetailEntity entity,
    string leagueName,
    IEnumerable<BetTypeWinLossModel> betTypeWinLoss,
    BetTypeWinLossModel total,
    string homeName,
    string awayName)
{
    public int LeagueId { get; } = entity.LeagueId;

    public string LeagueName { get; } = leagueName;

    public string HomeName { get; } = homeName;

    public string AwayName { get; } = awayName;

    public int MatchId { get; } = entity.MatchId;

    public string MatchName { get; }

    public int Race { get; } = entity.Race;

    public IEnumerable<BetTypeWinLossModel> BetTypeWinLoss { get; } = betTypeWinLoss;

    public BetTypeWinLossModel Total { get; } = total;

    public bool RenderSubTotal => MatchId != 0 || Race != 0;
}