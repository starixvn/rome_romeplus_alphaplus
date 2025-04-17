namespace AlphaPlus.Core.Models;

public class BelongingInfo
{
    public BelongingInfo() { }

    public BelongingInfo(byte roleId, bool isActive)
    {
        RoleId = roleId;
        IsActive = isActive;
    }

    public byte RoleId { get; set; }

    public bool IsActive { get; set; }

    public bool IsCrossLevel(byte uplineRoleId) => uplineRoleId - RoleId > 1;
}
