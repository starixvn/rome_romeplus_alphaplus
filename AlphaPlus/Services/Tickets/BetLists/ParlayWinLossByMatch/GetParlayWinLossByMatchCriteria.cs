using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.ParlayWinLossByMatch;

internal class GetParlayWinLossByMatchCriteria(
    int custId,
    int betType,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDate,
    UserLevel custLevel) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public override string GetSettingKey()
        => custLevel.IsSuperLevel()
        ? "Age_BetList_SelWinlossParlay_SAg"
        : custLevel.IsMasterLevel()
            ? "Age_BetList_SelWinlossParlay_MAg"
            : "Age_BetList_SelWinlossParlay_Ag";

    public override bool IsValid()
        => CustId > 0 && BetType > 0 && ToDate >= FromDate && FromDate > SqlDateTime.MinValue.Value;
}