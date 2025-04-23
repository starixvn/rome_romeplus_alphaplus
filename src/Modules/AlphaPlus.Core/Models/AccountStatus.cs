namespace AlphaPlus.Core.Models;

public class AccountStatus(bool isActive, byte locked)
{
    public bool IsActive { get; } = isActive;

    public bool IsClosed => !IsActive;

    public byte Locked { get; } = locked;

    public bool IsLocked => Locked != 0;
}
