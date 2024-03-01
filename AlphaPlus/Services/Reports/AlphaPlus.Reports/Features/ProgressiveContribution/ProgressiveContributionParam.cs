namespace AlphaPlus.Reports.Features.ProgressiveContribution;

public record ProgressiveContributionParam(
    int CustId,
    DateTime FromDate,
    DateTime ToDate,
    string ProductIds,
    UserLevel ViewerLevel,
    string SiteName);
