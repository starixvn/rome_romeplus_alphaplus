using Fanex.Data;

namespace AlphaPlus.Settings.Transfer.API.Criteria.MultipleTransfer;

public class GetSuperTransferListCriteria(
    int custId,
    string downlineName,
    string status,
    bool allYesterdayBalance,
    bool allowTotalBalanceTransfer,
    byte autoTransfer,
    int pageIndex,
    int pageSize) : GetTransferListCriteria(
        downlineName,
        status,
        allYesterdayBalance,
        allowTotalBalanceTransfer,
        autoTransfer,
        pageIndex,
        pageSize)
{
    [SpParam(SqlParamName = "sagid")]
    public int SuperId { get; } = custId;

    public decimal? TotalOutStanding { get; set; }

    public decimal? TotalCredit { get; set; }

    public decimal? TotalUsedCredit { get; set; }

    public decimal? TotalMAgentBalance { get; set; }

    public decimal? TotalYesterdayMAgentBalance { get; set; }

    public decimal? TotalMAgentCashBalance { get; set; }

    public decimal? TotalYesterday_MAgentCashBalance { get; set; }

    public decimal? TotalPendingTransfer3rd { get; set; }

    public override string GetSettingKey() => "Age_Transfer_SelList_SAg";

    public override bool IsValid() => SuperId > 0;
}