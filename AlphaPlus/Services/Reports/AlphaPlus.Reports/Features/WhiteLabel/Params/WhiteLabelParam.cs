namespace AlphaPlus.Reports.Features.WhiteLabel;

public record WhiteLabelParam(
    int ViewerId,
    DateTime FromDate,
    DateTime ToDate,
    string WhiteLabels,
    string Username,
    string SiteName);