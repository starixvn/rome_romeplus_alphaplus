using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.BetChoiceMatching;

public class GetBetChoiceMatchingCriteria(
    int custId,
    DateTime winlostDate,
    DateTime moveBetsDay,
    int betCount,
    bool isExportExcel) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime WinlostDate { get; } = winlostDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public int BetCount { get; } = betCount;

    public bool IsExportExcel { get; } = isExportExcel;

    public override string GetSettingKey() => "Age_BetStatistics_BetMatching_Master";

    public override bool IsValid() => CustId > 0;
}