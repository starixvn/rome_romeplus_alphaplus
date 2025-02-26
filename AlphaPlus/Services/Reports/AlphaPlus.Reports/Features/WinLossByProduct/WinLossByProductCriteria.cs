using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.WinLossByProduct;

internal class WinLossByProductCriteria(
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
        ? "Age_Rpt_WinLossByProduct_Member"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_Rpt_WinLossByProduct_Agent"
            : "Age_Rpt_WinLossByProduct_Master";
}