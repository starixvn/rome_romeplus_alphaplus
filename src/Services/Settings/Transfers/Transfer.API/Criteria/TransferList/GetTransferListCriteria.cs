namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferList;

using Core.Criteria;
using Fanex.Data;

public abstract class GetTransferListCriteria(
    string downlineName,
    string status,
    bool allYesterdayBalance,
    bool allowTotalBalanceTransfer,
    byte autoTransfer,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : SiteDbBaseCriteria(siteDbGroupId)
{
    public string Username { get; } = downlineName;

    public string CustStatus { get; } = status;

    [SpParam(SqlParamName = "IsYesterdayBalance")]
    public bool AllYesterdayBalance { get; } = allYesterdayBalance;

    [SpParam(SqlParamName = "IsAllowTotalBalance")]
    public bool AllowTotalBalanceTransfer { get; } = allowTotalBalanceTransfer;

    public byte AutoTransfer { get; } = autoTransfer;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalPages { get; set; }
}