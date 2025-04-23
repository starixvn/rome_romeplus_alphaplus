namespace AlphaPlus.Reports.Features.WinLossDetail;

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
    byte levelType) : WinLossDetailBaseCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_Rpt_WinlossDetail_WeekDataSel";
}