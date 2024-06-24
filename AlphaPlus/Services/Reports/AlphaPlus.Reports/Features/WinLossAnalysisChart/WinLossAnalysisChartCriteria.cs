using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.Reports.Features.WinLossAnalysisChart;

internal class WinLossAnalysisChartCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    UserLevel reportLevel) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    [SpParam(Ignored = true)]
    public UserLevel ReportLevel { get; } = reportLevel;

    public override string GetSettingKey()
        => ReportLevel.IsAgentLevel()
        ? "Age_Chart_WinlossAnalysis_Member"
        : ReportLevel.IsMasterLevel()
            ? "Age_Chart_WinlossAnalysis_Agent"
            : "Age_Chart_WinlossAnalysis_Master";
}