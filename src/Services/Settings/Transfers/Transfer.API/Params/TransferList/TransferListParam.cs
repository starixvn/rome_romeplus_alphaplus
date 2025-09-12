namespace AlphaPlus.Settings.Transfer.API.Params.TransferList;

public record TransferListParam(
    int CustId,
    byte CustLevelId,
    string DownlineName,
    string Status,
    byte AutoTransfer,
    byte LevelType,
    bool AllYesterdayBalance,
    bool AllowTotalBalanceTransfer,
    int PageIndex,
    int PageSize,
    string AGIP,
    int SiteDbGroupId);