using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.MemberBetList;

public class GetBetListAllByDateCriteria(
    int custId,
    string productIds,
    DateTime fromDate,
    DateTime toDate,
    string userName) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public string UserName { get; } = userName;

    public string ProductIds { get; } = productIds;

    public override string GetSettingKey() => "Age_BetList_SelAllByDate_Mem";

    public override bool IsValid()
        => CustId > 0
        && !string.IsNullOrWhiteSpace(ProductIds)
        && ToDate >= FromDate
        && FromDate > SqlDateTime.MinValue.Value;
}
