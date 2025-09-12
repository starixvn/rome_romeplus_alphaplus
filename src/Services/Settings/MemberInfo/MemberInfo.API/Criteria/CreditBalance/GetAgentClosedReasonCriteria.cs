namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetAgentClosedReasonCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize) : GetCreditBalanceCriteriaBase(custId, userName, enabled, closed, suspended, pageIndex, pageSize, 0)
{
    public override string GetSettingKey() => "Age_CustomerSelCreditBalClosedReason_Ag";
}