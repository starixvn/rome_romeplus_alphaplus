namespace AlphaPlus.Settings.Transfer.API.Criteria.Reward;

internal class GetRewardPaymentMasterCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    string programIds,
    bool? paymentStatus,
    byte levelType,
    string username) : GetRewardPaymentAgentCriteria(custId, fromDate, toDate, programIds, paymentStatus, levelType, username)
{
    public override string GetSettingKey() => "Age_RewardPayment_Master";
}