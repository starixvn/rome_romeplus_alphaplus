namespace AlphaPlus.Reports.Features.WinLossAnalysisChart;

public record WinLossAnalysisChartParam(
    int CustId,
    byte CustLevelId,
    string SiteName,
    int CurrencyId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds);