namespace AlphaPlus.Reports.Features.Consolidation;

public record ConsolidationParam(
    int CustId,
    int Currency,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    string ProductDetailsXml,
    string ReportType,
    byte SettingStatus,
    string SiteName,
    IEnumerable<ConsolidationCurrency> Currencies);
