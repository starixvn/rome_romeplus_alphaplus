
namespace AlphaPlus.Reports.Features.BetCreditTracking.BetCreditHistory;

public class BetCreditHistoryModel(
    int errorCode,
    string errorMessage,
    IEnumerable<BetCreditHistoryEntity> data,
    int totalRows)
{
    public int ErrorCode { get; } = errorCode;

    public string ErrorMessage { get; set; } = errorMessage;

    public IEnumerable<BetCreditHistoryEntity> ReportData { get; } = data;

    public int TotalRows { get; } = totalRows;
}