namespace AlphaPlus.Reports.Features.WinLossDetail;

using Core.Helpers;

internal class WinLossDetailMemberCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    bool isReward,
    string suffix) : WinLossDetailBaseCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward, suffix)
{
    public int PageIndex => 1;

    public int PageSize => 10000;

    public int TotalRecords { get; set; }

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_WinLossDetail_Member", Suffix);
}