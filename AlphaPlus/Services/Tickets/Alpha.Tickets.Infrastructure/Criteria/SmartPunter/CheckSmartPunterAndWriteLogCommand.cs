namespace Alpha.Tickets.Infrastructure.Criteria.SmartPunter;

internal class CheckSmartPunterAndWriteLogCommand(
    string usernames,
    bool isRestricted,
    int userId,
    string ip) : WriteSmartPunterLogCommand(usernames, isRestricted, userId, ip)
{
    public override string GetSettingKey()
        => "Age_CustomerSelCheckListSmartPunter";
}
