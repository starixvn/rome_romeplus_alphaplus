using Fanex.Data.Repository;

namespace AlphaPlus.Results.Criteria;

public class GetResultSportsByDateCriteria(DateTime moveBetsDay, bool localTest) : CriteriaBase
{
    public string EventDate { get; init; }

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public bool IsShowAll { get; } = localTest;

    public override string GetSettingKey() => "Age_Result_SportSelByDate";

    public override bool IsValid() => true;
}
