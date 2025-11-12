namespace AlphaPlus.Reports.Features.WinLossDetail;

internal class WinLossDetailMemberCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    bool isReward) : WinLossDetailBaseCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward)
{
    public int PageIndex { get; } = 1;

    public int PageSize { get; } = 10000;

    public int TotalRecords { get; set; }

    public override string GetSettingKey() => "Age_Rpt_WinLossDetail_Member";
}