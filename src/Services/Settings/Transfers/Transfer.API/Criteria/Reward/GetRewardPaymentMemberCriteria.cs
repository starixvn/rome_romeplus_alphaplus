namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class GetRewardPaymentMemberCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string programIds,
    bool? paymentStatus,
    string username) : GetRewardCriteria(custId, fromDate, toDate, programIds, paymentStatus, username)
{
    public override string GetSettingKey() => "Age_RewardPayment_Member";
}