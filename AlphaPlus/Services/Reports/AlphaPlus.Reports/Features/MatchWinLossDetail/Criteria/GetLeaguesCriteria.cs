using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.MatchWinLossDetail;

internal class GetLeaguesCriteria(
    int custId,
    string productIds,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    string language,
    bool isShowAll) : CriteriaBase
{
    public int CustId { get; } = custId;

    public string ProductIds { get; } = productIds;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public string Language { get; } = language;

    public bool IsShowAll { get; } = isShowAll;

    public override string GetSettingKey() => "Age_Rpt_MatchWinloss_GetLeagueByProduct";

    public override bool IsValid()
        => CustId > 0 && !string.IsNullOrWhiteSpace(ProductIds) && FromDate <= ToDate;
}