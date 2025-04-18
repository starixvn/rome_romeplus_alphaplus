namespace AlphaPlus.Core.Models;

public class BelongingInfo
{
    public byte RoleId { get; init; }

    public bool IsActive { get; init; }

    public bool IsCrossLevel(byte uplineRoleId) => uplineRoleId - RoleId > 1;
}
