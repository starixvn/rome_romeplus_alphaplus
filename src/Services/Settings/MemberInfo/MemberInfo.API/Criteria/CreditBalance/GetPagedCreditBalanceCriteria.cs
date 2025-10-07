namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public abstract class GetPagedCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : GetCreditBalanceCriteriaBase(custId, userName, enabled, closed, suspended, pageIndex, pageSize, siteDbGroupId)
{
    public int TotalItems { get; set; }
}