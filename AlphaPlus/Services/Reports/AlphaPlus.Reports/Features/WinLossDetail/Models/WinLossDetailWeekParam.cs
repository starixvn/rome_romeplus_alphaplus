namespace AlphaPlus.Reports.Features.WinLossDetail;

public record WinLossDetailWeekParam(
    int ViewerId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    string Username,
    bool IsReward,
    byte LevelType,
    string SiteName);