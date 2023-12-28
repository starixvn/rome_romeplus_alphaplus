using Fanex.Data.Repository;

namespace Alpha.Tickets.Infrastructure.Criteria.SmartPunter;

internal abstract class WriteSmartPunterLogCommandBase(bool isRestricted, int userId, string ip) : NonQueryCommand
{
    public int ActionId { get; } = 31008;

    public bool IsRestricted { get; } = isRestricted;

    public int UserId { get; } = userId;

    public string IP { get; } = ip;
}