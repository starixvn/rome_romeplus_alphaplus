using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.Reports.Features.WinLoss;

internal class WinLossCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    UserLevel custLevel,
    string productDetails
) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = custLevel;

    public string ProductDetails { get; } = productDetails;

    public override string GetSettingKey()
        => CustLevel.IsAgentLevel()
        ? "Age_Rpt_WinLoss_Member"
        : CustLevel.IsMasterLevel()
            ? "Age_Rpt_WinLoss_Agent"
            : "Age_Rpt_WinLoss_Master";
}