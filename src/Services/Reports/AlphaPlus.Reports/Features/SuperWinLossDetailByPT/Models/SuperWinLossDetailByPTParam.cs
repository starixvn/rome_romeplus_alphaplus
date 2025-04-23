using AlphaPlus.Reports.Features.WinLoss;

namespace AlphaPlus.Reports.Features.SuperWinLossDetailByPT;

public record SuperWinLossDetailByPTParam(
    int CustId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    byte SettingStatus,
    string SiteName) : WinLossParam(CustId, RoleIdConstants.Super, FromDate, ToDate, ProductIds, ProductDetailsXml, 0, SiteName);