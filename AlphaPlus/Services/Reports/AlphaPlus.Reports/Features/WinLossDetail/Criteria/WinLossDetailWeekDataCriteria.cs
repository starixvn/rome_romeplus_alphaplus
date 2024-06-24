using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.WinLossDetail;

internal class WinLossDetailWeekDataCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public string ProductDetails { get; } = productDetails;

    public string Username { get; } = username;

    public override string GetSettingKey() => "Age_Rpt_WinlossDetail_WeekDataSel";
}