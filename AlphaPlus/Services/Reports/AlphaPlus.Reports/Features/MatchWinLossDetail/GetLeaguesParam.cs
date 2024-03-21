namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public record GetLeaguesParam(
    int CustId,
    string ProductIds,
    DateTime FromDate,
    DateTime ToDate,
    string Language,
    bool IsShowAll);
