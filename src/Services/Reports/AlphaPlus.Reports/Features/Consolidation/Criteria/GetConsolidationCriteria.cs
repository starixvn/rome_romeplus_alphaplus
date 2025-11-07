namespace AlphaPlus.Reports.Features.Consolidation;

using Core.Helpers;
using Fanex.Data;
using Shared.Criteria;

internal class GetConsolidationCriteria(
    int custId,
    int currency,
    DateTime fromDate,
    DateTime toDate,
    byte reportType,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string suffix) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public byte ReportType { get; } = reportType;

    public int Currency { get; } = currency;

    public string ProductDetails { get; } = productDetails;

    [SpParam(Ignored = true)]
    public string Suffix { get; } = suffix;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_ConsolidatedGroup", Suffix);
}