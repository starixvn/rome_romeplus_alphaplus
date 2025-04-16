namespace AlphaPlus.Identity.Features.Authorization.Models;

public record BelongingInfo(byte RoleId, bool IsActive)
{
    public bool IsCrossLevel(byte uplineRoleId) => uplineRoleId - RoleId > 1;
}
