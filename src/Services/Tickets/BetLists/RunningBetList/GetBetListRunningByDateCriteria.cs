using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.RunningBetList;

public class GetBetListRunningByDateCriteria(
    int custId,
    string productIds,
    DateTime fromDate,
    DateTime toDate,
    bool getAll,
    string userName) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public bool GetAll { get; } = getAll;

    public string UserName { get; } = userName;

    public string ProductIds { get; } = productIds;

    public override string GetSettingKey() => "Age_BetList_SelRunningByDate_Mem";

    public override bool IsValid()
        => CustId > 0
        && !string.IsNullOrWhiteSpace(ProductIds)
        && ToDate >= FromDate
        && FromDate > SqlDateTime.MinValue.Value;
}
