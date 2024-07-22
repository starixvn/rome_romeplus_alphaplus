namespace AlphaPlus.Settings.Transfer.API.Params.TransferList;

public record TransferListParam(
    int CustId,
    byte CustLevelId,
    string DownlineName,
    string Status,
    byte AutoTransfer,
    bool AllYesterdayBalance,
    bool AllowTotalBalanceTransfer,
    int PageIndex,
    int PageSize,
    string AGIP);
