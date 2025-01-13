namespace AlphaPlus.Reports.Features.WinLossDetail;

internal class WinLossDetailMasterCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    string productDetails,
    string username,
    bool isReward,
    byte levelType) : WinLossDetailAgentCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward, levelType)
{
    public override string GetSettingKey() => "Age_Rpt_WinLossDetail_Master";
}