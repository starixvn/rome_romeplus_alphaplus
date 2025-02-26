using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.WinLossAnalysisChart;

internal class WinLossAnalysisChartCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    byte reportLevelId) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(reportLevelId)
        ? "Age_Chart_WinlossAnalysis_Member"
        : RoleIdConstants.IsMasterLevel(reportLevelId)
            ? "Age_Chart_WinlossAnalysis_Agent"
            : "Age_Chart_WinlossAnalysis_Master";
}