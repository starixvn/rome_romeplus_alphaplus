namespace AlphaPlus.Reports.Features.WinLoss;

public record WinLossParam(
    int CustId,
    byte CustLevelId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    string SiteName);