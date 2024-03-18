namespace AlphaPlus.Reports.Features.WinLossAnalysisChart;

public record WinLossAnalysisChartParam(
    int CustId,
    UserLevel CustLevel,
    string SiteName,
    int CurrencyId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds);
