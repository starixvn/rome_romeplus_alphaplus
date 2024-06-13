using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.CancelledBets;

public class GetCancelledBetCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    byte roleId,
    string productIds,
    string statusIds,
    int pageIndex,
    int pageSize) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public byte RoleId { get; } = roleId;

    public string ProductIds { get; } = productIds;

    public string StatusIdList { get; } = statusIds;

    public int PageIndex { get; } = pageIndex;

    public int PageSize { get; } = pageSize;

    public int TotalRows { get; set; }

    public override string GetSettingKey() => "Age_BetList_SelDeleted";

    public override bool IsValid()
        => CustId > 0
        && FromDate <= ToDate
        && FromDate > SqlDateTime.MinValue.Value
        && !string.IsNullOrWhiteSpace(ProductIds)
        && !string.IsNullOrWhiteSpace(StatusIdList);
}