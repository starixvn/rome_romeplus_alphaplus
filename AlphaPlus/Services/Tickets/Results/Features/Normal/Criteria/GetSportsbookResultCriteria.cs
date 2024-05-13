using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetSportsbookResultCriteria(
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

    public override string GetSettingKey() => "Age_Result_MatchSelByLeague";

    public override bool IsValid() => SportType > 0 && LeagueId >= 0;
}