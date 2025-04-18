namespace AlphaPlus.Core.Models;

public class DownlineInfo
{
    public bool IsActive { get; init; }

    public int CustId { get; init; }

    public bool IsSubAcc { get; init; }

    public byte RoleId { get; init; }

    public int AgentId { get; init; }
}