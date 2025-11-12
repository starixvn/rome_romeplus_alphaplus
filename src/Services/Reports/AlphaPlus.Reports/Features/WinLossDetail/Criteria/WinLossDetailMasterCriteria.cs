namespace AlphaPlus.Reports.Features.WinLossDetail;

using Core.Helpers;
using Criteria;

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
    byte levelType,
    string suffix) :
    WinLossDetailAgentCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward, levelType, suffix)
{
    public override string GetSettingKey() => SpNameHelpers.AppendSuffix("Age_Rpt_WinLossDetail_Master", Suffix);
}