namespace AlphaPlus.Reports.Features.WinLossAnalysis;

public record WinLossAnalysisParam(
    int ViewerId,
    UserLevel ViewerLevel,
    string SiteName,
    int ReportType,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds);