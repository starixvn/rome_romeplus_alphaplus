namespace AlphaPlus.BetLists.RunningBetListByBetTypeAndMatch;

public record RunningBetListByBetTypeAndMatchParam(
    int MatchId,
    int CustId,
    int BetType,
    string Language,
    int LiveIndicator,
    string BetTeam,
    byte CustLevelId,
    bool DisplayVNIP);