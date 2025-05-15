namespace AlphaPlus.CashService.API.Features.Shared.DTOs;

public class SiteDbConfig
{
    public int SiteDBGroupId { get; init; }

    public string LinkedServerName { get; init; }

    public string ClientAccessName { get; init; }

    public string AGIP { get; init; }

    public int CustDBGroupId { get; init; }

    public string AGIP_DBCUST { get; init; }
}