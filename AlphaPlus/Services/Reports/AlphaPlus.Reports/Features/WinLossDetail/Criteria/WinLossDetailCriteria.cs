namespace AlphaPlus.Reports.Features.WinLossDetail;

internal class WinLossDetailCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    byte custLevelId,
    string productDetails,
    string username,
    bool isReward)
        : WinLossDetailWeekDataCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds, productDetails, username, isReward)
{
    public int PageIndex { get; } = 1;

    public int PageSize { get; } = 10000;

    public int TotalRecords { get; set; }

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_Rpt_WinLossDetail_Member"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_Rpt_WinLossDetail_Agent"
            : "Age_Rpt_WinLossDetail_Master";
}