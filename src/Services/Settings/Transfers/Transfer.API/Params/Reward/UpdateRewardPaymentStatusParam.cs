using AlphaPlus.Settings.Transfer.API.Models.Reward;

namespace AlphaPlus.Settings.Transfer.API.Params.Reward;

public record UpdateRewardPaymentStatusParam(
    DateTime FromDate,
    DateTime ToDate,
    RewardPaymentStatus[] Data,
    int ActorId,
    int SubAccId,
    string SubAccName,
    string IP,
    string Language,
    int SiteDbGroupId);