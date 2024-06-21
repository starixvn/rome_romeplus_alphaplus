namespace AlphaPlus.BetLists.Shared.Models;

public record GetProductDetailsParam(
    int CustId,
    IEnumerable<Product> Products,
    int[] ProductOfferProductDetail,
    bool IsSiteLabel777);
