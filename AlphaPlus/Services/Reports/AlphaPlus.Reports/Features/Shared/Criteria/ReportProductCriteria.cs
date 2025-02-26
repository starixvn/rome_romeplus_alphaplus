namespace AlphaPlus.Reports.Features.Shared.Criteria;

internal abstract class ReportProductCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds) : ReportCriteriaBase(custId, fromDate, toDate, balanceUpDay, moveBetsDay)
{
    public string ProductIds { get; } = productIds;

    public override bool IsValid() => base.IsValid() && !string.IsNullOrWhiteSpace(ProductIds);
}
