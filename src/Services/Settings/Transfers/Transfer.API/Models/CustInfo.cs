namespace AlphaPlus.Settings.Transfer.API.Models;

public class CustInfo
{
    public int CustId { get; init; }

    public byte RoleId { get; init; }

    public string Username { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Username2 { get; init; }

    public int? AgentId { get; set; }
}