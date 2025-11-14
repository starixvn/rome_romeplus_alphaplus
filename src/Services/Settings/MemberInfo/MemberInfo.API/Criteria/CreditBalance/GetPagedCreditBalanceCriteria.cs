namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

public abstract class GetPagedCreditBalanceCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize) : GetCreditBalanceCriteriaBase(custId, userName, enabled, closed, suspended, pageIndex, pageSize)
{
    public int TotalItems { get; set; }
}