namespace AlphaPlus.BetLists.WinLoss;

public record WinLossWeekParam(
    int ViewerId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXML,
    string Username,
    string IP);
