namespace AlphaPlus.BetLists.DailyStatement;

internal class GetAllBetListByDayCriteria(
    int custId,
    DateTime winlossDate,
    DateTime moveBetsDay,
    int transType) : GetAllBetListByDayOldCriteria(custId, winlossDate, transType)
{
    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_BetList_SelByDayAll_Mem";
}