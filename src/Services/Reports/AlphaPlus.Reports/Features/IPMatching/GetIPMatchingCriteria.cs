using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.IPMatching;

internal class GetIPMatchingCriteria(int custId, DateTime winlostDate, DateTime moveBetsDay, int memberCount) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime WinlostDate { get; } = winlostDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public int MemberCount { get; } = memberCount;

    public override string GetSettingKey() => "Age_BetStatistics_IPMatching_Master";

    public override bool IsValid() => CustId > 0;
}