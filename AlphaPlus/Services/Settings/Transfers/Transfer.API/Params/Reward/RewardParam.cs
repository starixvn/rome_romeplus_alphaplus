using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Transfer.API.Params.Reward;

public record RewardParam(
    int CustId,
    UserLevel CustLevel,
    DateTime FromDate,
    DateTime ToDate,
    string ProgramIds,
    bool? PaymentStatus,
    string Username,
    int PageIndex = 0,
    int PageSize = 0);