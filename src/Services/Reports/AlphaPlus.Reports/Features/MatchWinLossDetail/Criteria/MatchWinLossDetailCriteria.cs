using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

internal class MatchWinLossDetailCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    string productIds,
    string leagueIds,
    string league3rdIds,
    byte reportLevelId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public string ProductIds { get; } = productIds;

    public string LeagueIds { get; } = leagueIds;

    public string League3rdIds { get; } = league3rdIds;

    public bool IsGetAll => string.IsNullOrWhiteSpace(LeagueIds) && string.IsNullOrEmpty(League3rdIds);

    public override string GetSettingKey()
        => RoleIdConstants.IsAgentLevel(reportLevelId)
        ? "Age_Rpt_MatchWinLossDetail_Member"
        : RoleIdConstants.IsMasterLevel(reportLevelId)
            ? "Age_Rpt_MatchWinLossDetail_Agent"
            : "Age_Rpt_MatchWinlossDetail_Master";

    public override bool IsValid()
        => CustId > 0 && FromDate <= ToDate && !string.IsNullOrWhiteSpace(ProductIds);
}