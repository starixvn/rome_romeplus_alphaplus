namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetMasterCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    byte levelType,
    int pageIndex,
    int pageSize) : GetPagedCreditBalanceCriteria(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
{

    public byte LevelType { get; } = levelType;

    public override string GetSettingKey() => "SiteDB_Age_CustomerSelCreditBalanceList_Mag";
}
