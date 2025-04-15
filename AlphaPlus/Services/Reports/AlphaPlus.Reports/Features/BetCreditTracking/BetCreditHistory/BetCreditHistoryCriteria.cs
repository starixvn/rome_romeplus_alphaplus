using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public class BetCreditHistoryCriteria(
    string username,
    DateTime fromDate,
    DateTime toDate,
    long? refNo,
    int loggedId,
    int pageIndex,
    int pageSize) : CriteriaBase
{
    public string Username { get; } = username;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public long? RefNo { get; } = refNo;

    public int LoggedId { get; } = loggedId;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRows { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMess { get; set; }

    public override string GetSettingKey() => "Age_Rpt_BetCreditHistory";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(Username) && FromDate <= ToDate;
}
