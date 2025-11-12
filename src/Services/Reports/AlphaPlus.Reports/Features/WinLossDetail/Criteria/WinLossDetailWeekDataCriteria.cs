namespace AlphaPlus.Reports.Features.WinLossDetail;

using Core.Helpers;

internal class WinLossDetailWeekDataCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    bool isReward,
    byte levelType,
    string suffix) : WinLossDetailBaseCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward, suffix)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_WinlossDetail_WeekDataSel", Suffix);
}