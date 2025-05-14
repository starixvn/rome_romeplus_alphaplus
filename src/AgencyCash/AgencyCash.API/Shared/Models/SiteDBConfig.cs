namespace AgencyCash.API.Shared.Models;

public class SiteDBConfig
{
    public int SiteDBGroupId { get; init; }

    public string LinkedServerName { get; init; }

    public string ClientAccessName { get; init; }

    public string AGIP { get; init; }

    public int CustDBGroupId { get; init; }
}