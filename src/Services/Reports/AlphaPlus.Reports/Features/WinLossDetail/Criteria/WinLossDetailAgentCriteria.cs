namespace AlphaPlus.Reports.Features.WinLossDetail;

internal class WinLossDetailAgentCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    bool isReward,
    byte levelType) : WinLossDetailMemberCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_Rpt_WinLossDetail_Agent";
}