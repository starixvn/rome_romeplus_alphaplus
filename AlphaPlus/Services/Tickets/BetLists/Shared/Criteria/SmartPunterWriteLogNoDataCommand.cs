using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class SmartPunterWriteLogNoDataCommand(
    int? custId,
    int actionId,
    bool isRestricted,
    int userId,
    string ip) : NonQueryCommand
{

    public int? CustId { get; } = custId;

    public int ActionId { get; } = actionId;

    public bool IsRestricted { get; } = isRestricted;

    public int UserId { get; } = userId;

    public string IP { get; } = ip;

    public override string GetSettingKey() => "NAP_UserLog_SmartPunterIns";

    public override bool IsValid() => !CustId.HasValue || CustId.Value > 0;
}
