namespace AlphaPlus.Reports.Features.WinLoss;

internal class WinLossMasterCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    byte levelType) : WinLossAgentCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, levelType)
{
    public override string GetSettingKey() => "Age_Rpt_WinLoss_Master";
}