namespace AlphaPlus.Identity.API.Features.Authorization.Requests;

public record BelongingQuery(int CustId, long TransId, int DownlineCustId, int SubAccId = 0);
