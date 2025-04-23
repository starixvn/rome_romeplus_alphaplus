namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetSuperCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    byte levelType,
    int pageIndex,
    int pageSize) : GetMasterCreditBalanceCriteria(custId, userName, enabled, closed, suspended, levelType, pageIndex, pageSize)
{
    public override string GetSettingKey() => "SiteDB_Age_CustomerSelCreditBalanceList_Sag";
}