namespace Alpha.Tickets.Infrastructure.Criteria.SmartPunter;

internal class WriteSmartPunterLogNoDataCommand(
    int? custId,
    bool isRestricted,
    int userId,
    string ip) : WriteSmartPunterLogCommandBase(isRestricted, userId, ip)
{
    public int? CustId { get; } = custId;

    public override string GetSettingKey()
        => "NAP_UserLog_SmartPunterIns";

    public override bool IsValid()
        => !CustId.HasValue || CustId.Value > 0;
}
