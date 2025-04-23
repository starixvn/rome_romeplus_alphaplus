namespace AlphaPlus.Reports.Features.BetCreditTracking.MoveStatementHistory;

public class MoveStatementHistoryModel(
    int errorCode,
    string errorMess,
    IEnumerable<MoveStatementHistoryByMember> reportData,
    MoveStatementHistorySummary summary,
    int totalRows)
{
    public int ErrorCode { get; } = errorCode;

    public string ErrorMessage { get; } = errorMess;

    public IEnumerable<MoveStatementHistoryByMember> ReportData { get; } = reportData;

    public MoveStatementHistorySummary Summary { get; } = summary;

    public int TotalRows { get; } = totalRows;
}
