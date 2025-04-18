namespace AlphaPlus.Core.Models;

public class AccountStatus
{
    public bool IsActive { get; init; }

    public bool IsClosed { get; init; }

    public byte Locked { get; init; }

    public bool IsLocked { get; init; }
}
