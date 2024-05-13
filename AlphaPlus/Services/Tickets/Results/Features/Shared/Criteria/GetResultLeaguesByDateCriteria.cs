namespace AlphaPlus.Results.Features.Shared;

public class GetResultLeaguesByDateCriteria(
    DateTime eventDate,
    DateTime moveBetsDay,
    bool isShowAll,
    int sportType) : GetResultSportsByDateCriteria(eventDate, moveBetsDay, isShowAll)
{
    public int SportType { get; } = sportType;

    public override string GetSettingKey() => "Age_Result_LeagueSelByDate";

    public override bool IsValid() => SportType > 0;
}
