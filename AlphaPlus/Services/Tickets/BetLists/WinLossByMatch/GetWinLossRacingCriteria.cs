using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLossByMatch;

internal class GetWinLossRacingCriteria(
    int custId,
    int betType,
    string race,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDate,
    UserLevel custLevel) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int BetType { get; } = betType;

    public string Race { get; } = race;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public override string GetSettingKey()
        => custLevel.IsSuperLevel()
        ? "Age_BetList_SelWinlossHR_SAg"
        : custLevel.IsMasterLevel()
            ? "Age_BetList_SelWinlossHR_MAg"
            : "Age_BetList_SelWinlossHR_Ag";

    public override bool IsValid()
        => CustId > 0 && ToDate >= FromDate && FromDate > SqlDateTime.MinValue.Value;
}