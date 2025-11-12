using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Transfer.API.Params.TransferYesterdayBalance;

public record AgentTransferMultipleYesterdayBalanceParam(
    int ActorId,
    UserLevel ActorLevel,
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
