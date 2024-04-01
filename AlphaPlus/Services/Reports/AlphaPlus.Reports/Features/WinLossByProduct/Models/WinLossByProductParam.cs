namespace AlphaPlus.Reports.Features.WinLossByProduct;

public record WinLossByProductParam(
    int CustId,
    UserLevel CustLevel,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailXml,
    string SiteName);