namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Core.Criteria;

public abstract class GetCreditBalanceCriteriaBase(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize,
    int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int CustId { get; } = custId;

    public string UserName { get; } = userName ?? string.Empty;

    public bool? Enabled { get; } = enabled;

    public bool? Closed { get; } = closed;

    public bool? Suspended { get; } = suspended;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public override bool IsValid() => CustId > 0;
}