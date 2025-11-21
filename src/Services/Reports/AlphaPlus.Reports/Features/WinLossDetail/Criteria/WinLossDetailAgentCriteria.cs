namespace AlphaPlus.Reports.Features.WinLossDetail.Criteria;

using AlphaPlus.Core.Helpers;

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
    byte levelType,
    string suffix) : WinLossDetailMemberCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward, suffix)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_WinLossDetail_Agent", Suffix);
}