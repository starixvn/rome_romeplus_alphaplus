namespace AlphaPlus.Settings.Transfer.API.Criteria.MultipleTransfer;

public class GetMasterTransferListCriteria(
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
    public int MasterId { get; } = custId;

    public decimal? TotalOutStanding { get; set; }

    public decimal? TotalCredit { get; set; }

    public decimal? TotalUsedCredit { get; set; }

    public decimal? TotalAgentBalance { get; set; }

    public decimal? TotalYesterdayAgentBalance { get; set; }

    public decimal? TotalAgentCashBalance { get; set; }

    public decimal? TotalYesterday_AgentCashBalance { get; set; }

    public decimal? TotalPendingTransfer3rd { get; set; }

    public override string GetSettingKey() => "Age_Transfer_SelList_MAg";

    public override bool IsValid() => MasterId > 0;
}