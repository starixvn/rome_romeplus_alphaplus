using Fanex.Data;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferList;

using Core.Helpers;

public class GetSuperTransferListCriteria(
    int custId,
    string downlineName,
    string status,
    bool allYesterdayBalance,
    bool allowTotalBalanceTransfer,
    byte autoTransfer,
    byte levelType,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : GetTransferListCriteria(
        downlineName,
        status,
        allYesterdayBalance,
        allowTotalBalanceTransfer,
        autoTransfer,
        pageIndex,
        pageSize,
        siteDbGroupId)
{
    [SpParam(SqlParamName = "sagid")]
    public int SuperId { get; } = custId;

    public byte LevelType { get; } = levelType;

    public decimal? TotalOutStanding { get; set; }

    public decimal? TotalCredit { get; set; }

    public decimal? TotalUsedCredit { get; set; }

    public decimal? TotalMAgentBalance { get; set; }

    public decimal? TotalYesterdayMAgentBalance { get; set; }

    public decimal? TotalMAgentCashBalance { get; set; }

    public decimal? TotalYesterday_MAgentCashBalance { get; set; }

    public decimal? TotalPendingTransfer3rd { get; set; }

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_SelList_SAg", SiteDbGroupId);

    public override bool IsValid() => SuperId > 0;
}