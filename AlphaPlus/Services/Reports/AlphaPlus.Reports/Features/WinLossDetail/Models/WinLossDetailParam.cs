using AlphaPlus.Reports.Features.WinLoss;

namespace AlphaPlus.Reports.Features.WinLossDetail;

public record WinLossDetailParam(
    int CustId,
    byte CustLevelId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    string Username,
    bool IsReward,
    string SiteName) : WinLossParam(CustId, CustLevelId, FromDate, ToDate, ProductIds, ProductDetailsXml, SiteName);
