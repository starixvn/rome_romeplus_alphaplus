namespace AlphaPlus.Settings.Transfer.API.Params.MultipleTransfer;

public record GetTransferListParam(
    int CustId,
    byte CustLevelId,
    string DonwlineName,
    string Status,
    byte AutoTransfer,
    bool AllYesterdayBalance,
    bool AllowTotalBalanceTransfer,
    int PageIndex,
    int PageSize);
