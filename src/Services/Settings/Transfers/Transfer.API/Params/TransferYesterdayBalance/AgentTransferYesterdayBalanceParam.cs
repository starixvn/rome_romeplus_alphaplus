namespace AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;

public record AgentTransferYesterdayBalanceParam(
    int AgentId, // incase direct member is super/master Id
    string AgentName, // incase direct member is super/master username
    int MemberId,
    decimal Amount,
    bool IsAllDayBalanceTransfer,
    string Language,
    string IP,
    int SubAccId,
    string AGIP,
    int SiteDbGroupId);