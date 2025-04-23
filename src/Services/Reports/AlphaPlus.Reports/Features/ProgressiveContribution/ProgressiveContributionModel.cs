namespace AlphaPlus.Reports.Features.ProgressiveContribution;

public record ProgressiveContributionModel(
    int ProductId,
    string ProductName,
    IEnumerable<ProgressiveContributionEntity> ProgressiveContributions,
    ProgressiveContributionEntity Total);
