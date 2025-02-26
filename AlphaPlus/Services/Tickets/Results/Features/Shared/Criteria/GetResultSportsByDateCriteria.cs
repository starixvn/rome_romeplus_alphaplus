using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Shared;

public class GetResultSportsByDateCriteria(
    DateTime eventDate,
    DateTime moveBetsDay,
    bool isShowAll) : CriteriaBase
{
    public DateTime EventDate { get; } = eventDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public bool IsShowAll { get; } = isShowAll;

    public override string GetSettingKey() => "Age_Result_SportSelByDate";

    public override bool IsValid() => true;
}
