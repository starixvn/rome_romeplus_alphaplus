namespace AlphaPlus.BetLists.Shared.Models;

public record GetProductDetailsParam(
    int CustId,
    HashSet<int> ProductOfferProductDetail,
    bool IsSiteLabel777);
