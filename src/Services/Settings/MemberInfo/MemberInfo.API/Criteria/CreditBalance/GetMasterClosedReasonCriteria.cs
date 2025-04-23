namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetMasterClosedReasonCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    byte levelType,
    int pageIndex,
    int pageSize) : GetAgentClosedReasonCriteria(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "Age_CustomerSelCreditBalClosedReason_MAg";
}
