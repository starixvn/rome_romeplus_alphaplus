using AlphaPlus.Reports.Features.Shared.Criteria;
using Fanex.Data;

namespace AlphaPlus.Reports.Features.ProgressiveContribution;

internal class ProgressiveContributionCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    UserLevel viewerLevel)
    : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    [SpParam(Ignored = true)]
    public UserLevel ViewerLevel { get; } = viewerLevel;

    public override string GetSettingKey()
        => ViewerLevel.IsAgentLevel()
        ? "Age_Rpt_ProgressiveGamesContribution_Member"
        : ViewerLevel.IsMasterLevel()
            ? "Age_Rpt_ProgressiveGamesContribution_Agent"
            : "Age_Rpt_ProgressiveGamesContribution_Master";
}
