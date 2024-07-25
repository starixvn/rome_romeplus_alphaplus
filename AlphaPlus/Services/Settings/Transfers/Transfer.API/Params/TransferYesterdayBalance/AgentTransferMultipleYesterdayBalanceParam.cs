namespace AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;

public record AgentTransferMultipleYesterdayBalanceParam(
    int AgentId,
    string SuperIdsTransferTotal,
    string MemberIds,
    string Username,
    string Status,
    byte AutoTransfer,
    bool AllYesterdayBalance,
    bool AllowTotalBalanceTransfer,
    bool IsAllPages,
    string Language,
    string IP,
    int SubAccId,
    string AGIP,
    int AdminId,
    string AdminName);
