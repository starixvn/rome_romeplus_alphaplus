namespace AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;

public record AgentTransferYesterdayBalanceParam(
    int AgentId,
    string AgentName,
    string SuperIdsTransferTotal,
    int MemberId,
    decimal Amount,
    bool IsAllDayBalanceTransfer,
    string Language,
    string IP,
    int SubAccId,
    string AGIP);
