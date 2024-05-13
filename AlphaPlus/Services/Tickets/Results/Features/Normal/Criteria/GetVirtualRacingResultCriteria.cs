using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetVirtualRacingResultCriteria(
    DateTime fromDate,
    int sportType,
    int leagueId,
    bool isShowAll,
    DateTime moveBetsDay) : CriteriaBase
{
    public DateTime FromDate { get; } = fromDate;

    public int SportType { get; } = sportType;

    public int LeagueId { get; } = leagueId;

    public bool IsShowAll { get; } = isShowAll;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_Result_VSRaceSelByMeeting";

    public override bool IsValid() => FromDate > DateTime.MinValue && SportType > 0;
}