namespace AlphaPlus.Settings.MemberInfo.API.Criteria.CreditBalance;

using Fanex.Data.Repository;

public class GetAgentClosedReasonCriteria(
    int custId,
    string userName,
    bool? enabled,
    bool? closed,
    bool? suspended,
    int pageIndex,
    int pageSize) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string UserName { get; } = userName ?? string.Empty;

    public bool? Enabled { get; } = enabled;

    public bool? Closed { get; } = closed;

    public bool? Suspended { get; } = suspended;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public override bool IsValid() => CustId > 0;

    public override string GetSettingKey() => "Age_CustomerSelCreditBalClosedReason_Ag";
}