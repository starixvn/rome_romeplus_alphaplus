namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferList;

using Core.Helpers;

public class GetAgentTransferListCriteria(
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
    public int CustId { get; } = custId;

    public decimal? TotalOutStanding { get; set; }

    public decimal? TotalCredit { get; set; }

    public decimal? TotalBalance { get; set; }

    public decimal? TotalYesterdayBalance { get; set; }

    public decimal? TotalPendingTransfer3rd { get; set; }

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_SelList_Ag_ForDBTrans", SiteDbGroupId);

    public override bool IsValid() => CustId > 0;
}