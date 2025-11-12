namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Core.Helpers;

public class GetAgentCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : GetPagedCreditBalanceCriteria(custId, userName, enabled, closed, suspended, pageIndex, pageSize, siteDbGroupId)
{
    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_CustomerSelCreditBalanceList_Ag", SiteDbGroupId);
}