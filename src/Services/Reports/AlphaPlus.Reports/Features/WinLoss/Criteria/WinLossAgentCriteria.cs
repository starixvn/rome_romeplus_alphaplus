namespace AlphaPlus.Reports.Features.WinLoss;

internal class WinLossAgentCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    byte levelType) : WinLossMemberCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_Rpt_WinLoss_Agent";
}
