using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Criteria;

public class SmartPunterWriteLogCommand(
    string listUserName,
    int actionId,
    bool isRestricted,
    int userId,
    string ip) : NonQueryCommand
{
    public string ListUserName { get; } = listUserName;

    public int ActionId { get; } = actionId;

    public bool IsRestricted { get; } = isRestricted;

    public int UserId { get; } = userId;

    public string IP { get; } = ip;

    public override string GetSettingKey() => "Age_UserLog_SmartPunterInsMultiple";

    public override bool IsValid() => !string.IsNullOrEmpty(ListUserName);
}