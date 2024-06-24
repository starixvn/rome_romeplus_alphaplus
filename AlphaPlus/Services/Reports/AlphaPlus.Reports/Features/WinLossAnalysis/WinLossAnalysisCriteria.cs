using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.Reports.Features.WinLossAnalysis;

internal class WinLossAnalysisCriteria(int custId,
    int reportType,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    UserLevel reportLevel) : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    [SpParam(Ignored = true)]
    public UserLevel ReportLevel { get; } = reportLevel;

    public int ReportType { get; } = reportType;

    public override string GetSettingKey()
        => ReportLevel.IsAgentLevel()
        ? "Age_Rpt_WinlossAnalysis_Member"
        : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_WinlossAnalysis_Agent"
            : "Age_Rpt_WinlossAnalysis_Master";
}