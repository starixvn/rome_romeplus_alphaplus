using _UserLevel = AlphaPlus.Core.Models.UserLevel;

namespace AlphaPlus.BetLists.Params;

public class RunningBetListByBetTypeAndMatchParam(
    int matchId,
    int custId,
    int betType,
    string language,
    int liveIndicator,
    string betTeam,
    _UserLevel custLevel,
    bool displayVNIP)
{
    public int MatchId { get; } = matchId;

    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public string Language { get; } = language;

    public int LiveIndicator { get; } = liveIndicator;

    public string BetTeam { get; } = betTeam;

    public _UserLevel CustLevel { get; } = custLevel;

    public bool DisplayVNIP { get; } = displayVNIP;
}