namespace AlphaPlus.Results.Features.Normal;

using Fanex.Data.Repository;
public class GetVirtualRacing3rdResultDetailsCriteria(
    int eventId, 
    DateTime eventDate, 
    DateTime moveBetsDay, 
    DateTime balanceUpDay) : CriteriaBase
{
    public int EventId { get; } = eventId;

    public DateTime EventDate { get; set; } = eventDate;

    public DateTime MoveBetsDay { get; set; } = moveBetsDay;

    public DateTime BalanceUpDay { get; set; } = balanceUpDay;

    public override string GetSettingKey() => "Age_BetList_SelRaceResult";

    public override bool IsValid() => EventId >= 0;
}