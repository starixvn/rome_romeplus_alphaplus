namespace AlphaPlus.Reports.Features.WinLossAnalysis;

public record WinLossAnalysisParam(
    int ViewerId,
    byte ViewerLevelId,
    string SiteName,
    int ReportType,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds);