namespace AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;

public record TransferYesterdayBalanceParam(
    int CustId,
    byte CustLevelId,
    string SuperIdsTransferTotal,
    string ToCustIds,
    bool IsFullTransfer,
    decimal Amount,
    string CustStatus,
    byte AutoTransfer,
    string Language,
    string IP,
    int SubAccId,
    string AGIP);
