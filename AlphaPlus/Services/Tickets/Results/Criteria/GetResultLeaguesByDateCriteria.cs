using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.Results.Criteria;

public class GetResultLeaguesByDateCriteria(DateTime moveBetsDay, bool localTest) : CriteriaBase
{
    public string EventDate { get; init; }

    [SpParam(SqlParamName = "sporttype")]
    public int SportId { get; init; }

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public bool IsShowAll { get; } = localTest;

    public override string GetSettingKey() => "Age_Result_LeagueSelByDate";

    public override bool IsValid() => SportId > 0;
}
