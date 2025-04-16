namespace AlphaPlus.Identity.Features.Authorization.Models;

public record CustomerStatus(bool IsActive, byte Locked)
{
    public bool IsClosed { get; } = !IsActive;

    public bool IsLocked { get; } = Locked != 0;
}
