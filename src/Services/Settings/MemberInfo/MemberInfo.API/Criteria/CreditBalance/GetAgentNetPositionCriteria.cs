namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Core.Helpers;

public class GetAgentNetPositionCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize,
    int siteDbGroupId)
    : GetCreditBalanceCriteriaBase(custId, userName, enabled, closed, suspended, pageIndex, pageSize, siteDbGroupId)
{
    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_CustomerSelCreditBalNetPosition_Ag", SiteDbGroupId);
}