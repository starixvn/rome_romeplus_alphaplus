namespace AlphaPlus.Reports.Features.WinLoss;

public record WinLossParam(
    int CustId,
    UserLevel CustLevel,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    string SiteName);