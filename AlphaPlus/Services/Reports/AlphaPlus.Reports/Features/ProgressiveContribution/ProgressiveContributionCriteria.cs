using AlphaPlus.Reports.Features.Shared.Criteria;

namespace AlphaPlus.Reports.Features.ProgressiveContribution;

internal class ProgressiveContributionCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime balanceUpDay,
    DateTime moveBetsDay,
    string productIds,
    byte viewerLevelId)
    : ReportProductCriteria(custId, fromDate, toDate, balanceUpDay, moveBetsDay, productIds)
{
    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(viewerLevelId)
        ? "Age_Rpt_ProgressiveGamesContribution_Member"
        : RoleIdConstants.IsMasterLevel(viewerLevelId)
            ? "Age_Rpt_ProgressiveGamesContribution_Agent"
            : "Age_Rpt_ProgressiveGamesContribution_Master";
}
