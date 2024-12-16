using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Normal;

public class GetVirtuallySportResultCriteria(
    DateTime fromDate,
    int leagueId,
    DateTime moveBetsDay) : CriteriaBase
{
    public DateTime FromDate { get; } = fromDate;

    public int LeagueId { get; } = leagueId;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_Result_VS5_Match3rdSelByLeague";

    public override bool IsValid()
            => FromDate > DateTime.MinValue && LeagueId >= 0 && MoveBetsDay > DateTime.MinValue;

}