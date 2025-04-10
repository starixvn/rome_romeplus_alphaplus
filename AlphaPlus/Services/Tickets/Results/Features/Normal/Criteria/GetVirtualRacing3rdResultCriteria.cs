namespace AlphaPlus.Results.Features.Normal;

using Fanex.Data.Repository;
public class GetVirtualRacing3rdResultCriteria(
    DateTime fromDate,
    int sportType,
    int leagueId,
    DateTime moveBetsDay) : CriteriaBase
{
    public DateTime EventDate { get; } = fromDate;

    public int SportType { get; } = sportType;

    public int LeagueId { get; } = leagueId;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_Result_RaceSelByLeague";

    public override bool IsValid() => EventDate > DateTime.MinValue && SportType > 0;
}