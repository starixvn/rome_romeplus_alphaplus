namespace AlphaPlus.Reports.Features.WinLossByProduct;

public record WinLossByProductParam(
    int CustId,
    byte CustLevelId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailXml,
    string SiteName);