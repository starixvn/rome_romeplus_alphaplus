namespace AlphaPlus.Identity.API.Features.Authorization;

public record CustomerStatus(bool IsActive, byte Locked)
{
    public bool IsClosed { get; } = !IsActive;

    public bool IsLocked { get; } = Locked != 0;
}
