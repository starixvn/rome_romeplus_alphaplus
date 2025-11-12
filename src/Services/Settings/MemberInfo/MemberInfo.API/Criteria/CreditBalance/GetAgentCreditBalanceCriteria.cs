namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetAgentCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize) : GetPagedCreditBalanceCriteria(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
{
    public override string GetSettingKey() => "SiteDB_Age_CustomerSelCreditBalanceList_Ag";
}
