namespace AlphaPlus.Core.Models;

public class AccountStatus
{
    public AccountStatus() { }

    public AccountStatus(bool isActive, byte locked)
    {
        IsActive = isActive;
        Locked = locked;
    }

    public bool IsActive { get; set; }

    public bool IsClosed { get; set; }

    public byte Locked { get; set; }

    public bool IsLocked { get; set; }
}
