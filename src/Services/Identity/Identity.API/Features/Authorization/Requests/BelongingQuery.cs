namespace AlphaPlus.Identity.API.Features.Authorization.Requests;

public class BelongingQuery
{
    public int CustId { get; init; }

    public int SubAccId { get; init; }

    public long TransId { get; init; }

    public int DownlineCustId { get; init; }
}
