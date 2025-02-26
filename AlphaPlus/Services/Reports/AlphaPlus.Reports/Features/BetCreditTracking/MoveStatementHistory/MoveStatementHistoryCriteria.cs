using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

internal class MoveStatementHistoryCriteria(
    string username,
    DateTime fromDate,
    DateTime toDate,
    int loggedId,
    int pageIndex,
    int pageSize,
    bool isVariance) : CriteriaBase
{
    public string Username { get; } = username;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public int LoggedId { get; } = loggedId;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public bool IsVariance { get; } = isVariance;

    public int ErrorCode { get; set; }

    public string ErrorMess { get; set; }

    public int TotalRows { get; set; }

    public override string GetSettingKey() => "Age_Rpt_StatementMovement";

    public override bool IsValid() => FromDate <= ToDate;
}
