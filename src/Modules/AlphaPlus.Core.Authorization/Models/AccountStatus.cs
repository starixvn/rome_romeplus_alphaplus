namespace AlphaPlus.Core.Authorization.Models;

public class AccountStatus
{
    public bool IsActive { get; init; }

    public byte Locked { get; init; }

    public bool IsClosed => !IsActive;

    public bool IsLocked => Locked != 0;
}
