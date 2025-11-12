namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Core.Helpers;

public class GetMasterCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    byte levelType,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : GetPagedCreditBalanceCriteria(custId, userName, enabled, closed, suspended, pageIndex, pageSize, siteDbGroupId)
{
    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_CustomerSelCreditBalanceList_Mag", SiteDbGroupId);
}