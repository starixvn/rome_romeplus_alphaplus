namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class GetRewardPaymentAgentCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string programIds,
    bool? paymentStatus,
    byte levelType,
    string username) : GetRewardCriteria(custId, fromDate, toDate, programIds, paymentStatus, username)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_RewardPayment_Agent";
}