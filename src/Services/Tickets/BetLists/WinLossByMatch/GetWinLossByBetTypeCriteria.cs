using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLossByMatch;

internal class GetWinLossByBetTypeCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDate,
    byte custLevelId,
    int betType) : CriteriaBase
{
    public int CustId { get; } = custId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public byte RoleId { get; } = custLevelId;

    public int BetType { get; } = betType;

    public override string GetSettingKey() => "Age_BetList_SelWinLossByBetType";

    public override bool IsValid()
        => CustId > 0 && ToDate >= FromDate && FromDate > SqlDateTime.MinValue.Value;
}
