using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

public class MatchWinLossDetailCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    string productIds,
    string leagueIds,
    UserLevel reportLevel) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public string ProductIds { get; } = productIds;

    public string LeagueIds { get; } = leagueIds;

    public bool IsGetAll => string.IsNullOrWhiteSpace(LeagueIds);

    [SpParam(Ignored = true)]
    public UserLevel ReportLevel { get; } = reportLevel;

    public override string GetSettingKey()
        => ReportLevel.IsAgentLevel()
        ? "Age_Rpt_MatchWinLossDetail_Member"
        : ReportLevel.IsMasterLevel()
            ? "Age_Rpt_MatchWinLossDetail_Agent"
            : "Age_Rpt_MatchWinlossDetail_Master";

    public override bool IsValid()
        => CustId > 0 && FromDate <= ToDate && !string.IsNullOrWhiteSpace(ProductIds);
}