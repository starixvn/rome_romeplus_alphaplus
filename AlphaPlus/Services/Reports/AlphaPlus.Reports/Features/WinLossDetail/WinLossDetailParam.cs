using AlphaPlus.Reports.Features.WinLoss;

namespace AlphaPlus.Reports.Features.WinLossDetail;

public record WinLossDetailParam(
    int CustId,
    UserLevel CustLevel,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    string Username,
    string SiteName) : WinLossParam(CustId, CustLevel, FromDate, ToDate, ProductIds, ProductDetailsXml, SiteName)
{
}