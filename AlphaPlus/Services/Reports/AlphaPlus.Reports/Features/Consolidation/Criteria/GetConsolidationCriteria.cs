using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.Consolidation;

public class GetConsolidationCriteria(
    int custId,
    int currency,
    DateTime fromDate,
    DateTime toDate,
    byte reportType,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public byte ReportType { get; } = reportType;

    public int Currency { get; } = currency;

    public string ProductDetails { get; } = productDetails;

    public override string GetSettingKey() => "Age_Rpt_ConsolidatedGroup";
}