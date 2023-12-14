namespace AlphaPlus.Core.Configurations;

public class GroupPageAccess
{
    public int RoleId { get; set; }

    public string Name { get; set; }

    public string[] DeniedPages { get; set; }

    public PageAccess[] PageAccesses { get; set; }
}
