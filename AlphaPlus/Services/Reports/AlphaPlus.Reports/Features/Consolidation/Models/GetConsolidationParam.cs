namespace AlphaPlus.Reports.Features.Consolidation;

public record GetConsolidationParam(
    int CustId,
    int Currency,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    byte SettingStatus,
    string SiteName);