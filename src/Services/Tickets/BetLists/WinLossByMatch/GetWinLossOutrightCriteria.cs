using System.Data.SqlTypes;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.WinLossByMatch;

internal class GetWinLossOutrightCriteria(
    int custId,
    int leagueId,
    string language,
    DateTime fromDate,
    DateTime toDate,
    DateTime moveBetsDate,
    byte custLevelId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public int LeagueId { get; } = leagueId;

    public string Language { get; } = language;

    public DateTime FromDate { get; } = fromDate;

    public DateTime ToDate { get; } = toDate;

    public DateTime MoveBetsDay { get; } = moveBetsDate;

    public override string GetSettingKey()
        => RoleIdConstants.IsSuperLevel(custLevelId)
        ? "Age_BetList_SelWinlossOutright_SAg"
        : RoleIdConstants.IsMasterLevel(custLevelId)
            ? "Age_BetList_SelWinlossOutright_MAg"
            : "Age_BetList_SelWinlossOutright_Ag";

    public override bool IsValid()
        => CustId > 0 && ToDate >= FromDate && FromDate > SqlDateTime.MinValue.Value;
}
