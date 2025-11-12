namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public class GetAgentNetPositionCriteria : GetCreditBalanceCriteriaBase
{
    public GetAgentNetPositionCriteria(
        int custId,
        string userName,
        bool? enabled,
        bool? closed,
        bool? suspended,
        int pageIndex,
        int pageSize)
        : base(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
    {
    }

    public override string GetSettingKey() => "SiteDB_Age_CustomerSelCreditBalNetPosition_Ag";
}
