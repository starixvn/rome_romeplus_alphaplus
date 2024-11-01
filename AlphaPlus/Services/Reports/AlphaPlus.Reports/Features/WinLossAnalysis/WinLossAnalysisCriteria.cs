using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.WinLossAnalysis;

internal class WinLossAnalysisCriteria(int custId,
    int reportType,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    byte reportLevelId) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public int ReportType { get; } = reportType;

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(reportLevelId)
        ? "Age_Rpt_WinlossAnalysis_Member"
        : RoleIdConstants.IsMasterLevel(reportLevelId)
            ? "Age_Rpt_WinlossAnalysis_Agent"
            : "Age_Rpt_WinlossAnalysis_Master";
}