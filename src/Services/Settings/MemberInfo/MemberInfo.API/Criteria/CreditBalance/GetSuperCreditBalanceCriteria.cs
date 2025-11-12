namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Core.Helpers;

public class GetSuperCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    byte levelType,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : GetMasterCreditBalanceCriteria(custId, userName, enabled, closed, suspended, levelType, pageIndex, pageSize, siteDbGroupId)
{
    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_CustomerSelCreditBalanceList_Sag", SiteDbGroupId);
}