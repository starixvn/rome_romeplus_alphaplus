namespace AlphaPlus.Identity.API.Features.Authorization.Requests;

public class BelongingMultipleQuery
{
    public int CustId { get; init; }

    public int SubAccId { get; init; }

    public string CustIds { get; init; }
}