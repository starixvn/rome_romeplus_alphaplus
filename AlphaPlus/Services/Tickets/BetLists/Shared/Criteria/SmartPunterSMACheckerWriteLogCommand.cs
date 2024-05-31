namespace AlphaPlus.BetLists.Shared.Criteria;

public class SmartPunterSMACheckerWriteLogCommand(
    string listUserName,
    int actionId,
    bool isRestricted,
    int userId,
    string ip) : SmartPunterWriteLogCommand(listUserName, actionId, isRestricted, userId, ip)
{
    public override string GetSettingKey() => "Age_CustomerSelCheckListSmartPunter";
}