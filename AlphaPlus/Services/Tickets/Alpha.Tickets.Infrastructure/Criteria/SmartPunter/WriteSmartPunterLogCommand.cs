namespace Alpha.Tickets.Infrastructure.Criteria.SmartPunter;

internal class WriteSmartPunterLogCommand(
    string usernames,
    bool isRestricted,
    int userId,
    string ip) : WriteSmartPunterLogCommandBase(isRestricted, userId, ip)
{
    public string ListUserName { get; } = usernames;

    public override string GetSettingKey()
        => "Age_UserLog_SmartPunterInsMultiple";

    public override bool IsValid()
        => !string.IsNullOrEmpty(ListUserName);
}
