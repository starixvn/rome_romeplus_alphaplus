namespace Alpha.Tickets.Infrastructure.Criteria.BetList;

internal class GetBetListRunningByDateCriteria(
    int custId,
    string productIds,
    DateTime fromDate,
    DateTime toDate,
    bool getAll,
    string username
) : GetBetListAllByDateCriteria(custId, productIds, fromDate, toDate, username)
{
    public bool GetAll { get; } = getAll;

    public override string GetSettingKey()
        => "Age_BetList_SelRunningByDate_Mem";
}
