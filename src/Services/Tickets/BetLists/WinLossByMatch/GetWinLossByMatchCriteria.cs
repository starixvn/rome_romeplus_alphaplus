using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLossByMatch;

internal class GetWinLossByMatchCriteria(
    int custId,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDay,
    int matchId,
    byte eventType,
    int betType,
    string custName,
    bool liveIndicator,
    byte custLevelId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int MatchId { get; } = matchId;

    public byte EventType { get; } = eventType;

    public int BetType { get; } = betType;

    public string Username { get; } = custName;

    public bool LiveIndicator { get; } = liveIndicator;

    public byte RoleId { get; } = custLevelId;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public override string GetSettingKey() => "Age_BetList_SelWinlossByBettypeMatch";

    public override bool IsValid()
        => CustId > 0 && FromDate <= ToDate && SqlDateTime.MinValue.Value < FromDate;
}
