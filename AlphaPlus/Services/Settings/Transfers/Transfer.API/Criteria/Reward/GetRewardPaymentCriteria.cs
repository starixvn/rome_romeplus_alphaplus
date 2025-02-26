using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class GetRewardPaymentCriteria(
    int custId,
    UserLevel custLevel,
    DateTime fromDate,
    DateTime toDate,
    string programIds,
    bool? paymentStatus,
    string username) : GetRewardCriteria(custId, custLevel, fromDate, toDate, programIds, paymentStatus, username)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_RewardPayment_Master"
        : CustLevel.IsMasterLevel()
            ? "Age_RewardPayment_Agent"
            : "Age_RewardPayment_Member";
}