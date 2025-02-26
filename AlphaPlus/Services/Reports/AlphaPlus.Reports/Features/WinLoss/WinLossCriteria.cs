using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.WinLoss;

internal class WinLossCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    byte custLevelId,
    string productDetails) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public string ProductDetails { get; } = productDetails;

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(custLevelId)
        ? "Age_Rpt_WinLoss_Member"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_Rpt_WinLoss_Agent"
            : "Age_Rpt_WinLoss_Master";
}