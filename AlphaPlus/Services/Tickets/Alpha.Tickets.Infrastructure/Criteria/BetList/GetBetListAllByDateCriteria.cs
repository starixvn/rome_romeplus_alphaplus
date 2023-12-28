using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.BetList;

internal class GetBetListAllByDateCriteria(
    int custId,
    string productIds,
    DateTime fromDate,
    DateTime toDate,
    string username) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public string Username { get; } = username;

    public string ProductIds { get; } = productIds;

    public override string GetSettingKey()
        => "Age_BetList_SelAllByDate_Mem";

    public override bool IsValid()
        => CustId > 0
        && !string.IsNullOrWhiteSpace(ProductIds)
        && ToDate >= FromDate
        && FromDate > SqlDateTime.MinValue.Value;
}
