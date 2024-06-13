namespace AlphaPlus.BetLists.WinLoss;

public record WinLossWeekDataParam(
    int ViewerId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXML,
    string Username);
