namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Core.Helpers;

public class GetAgentNetPositionCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize)
    : GetCreditBalanceCriteriaBase(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
{
    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_CustomerSelCreditBalNetPosition_Ag", SiteDbGroupId);
}