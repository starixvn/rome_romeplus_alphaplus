namespace AlphaPlus.Core.Models;

public class BelongingInfo(byte roleId, bool isActive)
{
    public byte RoleId { get; } = roleId;

    public bool IsActive { get; } = isActive;

    public bool IsCrossLevel(byte uplineRoleId)
        => uplineRoleId - RoleId > 1;
}
