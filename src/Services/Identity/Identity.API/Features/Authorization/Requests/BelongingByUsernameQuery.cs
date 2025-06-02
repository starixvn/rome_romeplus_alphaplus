namespace AlphaPlus.Identity.API.Features.Authorization.Requests;

public class BelongingByUsernameQuery
{
    public int CustId { get; init; }

    public int SubAccId { get; init; }

    public string Username { get; init; }
}