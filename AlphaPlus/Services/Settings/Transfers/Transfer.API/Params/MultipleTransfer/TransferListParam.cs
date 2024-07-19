namespace AlphaPlus.Settings.Transfer.API.Params.MultipleTransfer;

public record TransferListParam(
    int CustId,
    byte CustLevelId,
    string DonwlineName,
    string Status,
    byte AutoTransfer,
    bool AllYesterdayBalance,
    bool AllowTotalBalanceTransfer,
    int PageIndex,
    int PageSize,
    string AGIP);
