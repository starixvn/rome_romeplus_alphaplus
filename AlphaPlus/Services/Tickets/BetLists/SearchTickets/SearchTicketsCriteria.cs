using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.SearchTickets;

internal class SearchTicketsCriteria(
    long refNo,
    int uplineId,
    byte roleId,
    DateTime moveBetsDay,
    DateTime balanceUpDay) : CriteriaBase
{
    public long RefNo { get; } = refNo;

    public int UplineId { get; } = uplineId;

    public byte RoleId { get; } = roleId;

    public DateTime MoveBetsDay { get; } = moveBetsDay;

    public DateTime BalanceUpDay { get; } = balanceUpDay;

    public override string GetSettingKey() => "Age_BetList_SearchByRefNo";

    public override bool IsValid() => RefNo > 0 && UplineId > 0;
}