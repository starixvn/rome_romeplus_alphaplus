using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class MatchWinLossDetailModel
{
    public MatchWinLossDetailModel(
        MatchWinLossDetailEntity entity,
        string leagueName,
        IEnumerable<BetTypeWinLossModel> betTypeWinLoss,
        BetTypeWinLossModel total,
        string homeName,
        string awayName)
    {
        LeagueId = entity.LeagueId;
        HomeName = homeName;
        AwayName = awayName;
        MatchId = entity.MatchId;
        Race = entity.Race;
        BetTypeWinLoss = betTypeWinLoss;
        Total = total;

        var (matchName, leagueName2) = BuildMatchAndLeagueName(entity, leagueName);
        LeagueName = leagueName2;
        MatchName = matchName;
    }

    public int LeagueId { get; }

    public string LeagueName { get; }

    public string HomeName { get; }

    public string AwayName { get; }

    public int MatchId { get; }

    public string MatchName { get; }

    public int Race { get; }

    public IEnumerable<BetTypeWinLossModel> BetTypeWinLoss { get; }

    public BetTypeWinLossModel Total { get; }

    public bool RenderSubTotal => MatchId != 0 || Race != 0;

    private (string, string) BuildMatchAndLeagueName(MatchWinLossDetailEntity data, string leagueName)
    {
        string matchName;
        string leagueName2 = leagueName;

        if (data.BetType == BetTypeConstants.Outright)
        {
            matchName = string.Empty;
        }
        else if (BetTypeConstants.IsNumberGame(data.BetType))
        {
            matchName = $"{SharedResources.NumberGameNo} {data.MatchCode}";
            leagueName2 = string.IsNullOrWhiteSpace(leagueName)
                ? SharedResources.NumberGame
                : $"{SharedResources.NumberGame} - {leagueName}";
        }
        else if (BetTypeConstants.IsHappy5(data.BetType))
        {
            matchName = $"{SharedResources.No} {data.MatchCode}";
            leagueName2 = string.IsNullOrWhiteSpace(leagueName)
                ? SharedResources.Happy5
                : $"{SharedResources.Happy5} - {leagueName}";
        }
        else if (BetTypeConstants.IsVirtualHorseRacing(data.BetType))
        {
            matchName = DateTime.TryParse(data.ShowTime, out var eventTime) ? eventTime.ToLongDateWithSlash() : string.Empty;
        }
        else
        {
            matchName = $"{HomeName}{(data.IsNeutral ? "(N)" : string.Empty)} - vs - {AwayName}";
        }

        return (matchName, leagueName2);
    }
}