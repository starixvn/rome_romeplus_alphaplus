using Fanex.RemotingRef;

namespace AlphaPlus.BetLists.Shared.Helpers;

public static class CacheRefServiceHelper
{
    internal static IRemotingRefService Instance { get; set; }

    public static string GetSportName(int sportTypeId)
        => Instance.GetSportName(sportTypeId.ToString(), LanguageCode, string.Empty);

    public static string GetBetTypeName(int betTypeId)
        => Instance.GetBetTypeName(betTypeId.ToString(), LanguageCode);

    public static string GetLeagueName(int leagueId)
        => Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

    public static string GetLeagueName(int leagueId, int leagueGroupId)
        => Instance.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty);

    public static string GetProductName(int productId, bool isSiteLabel777)
    {
        var productName = isSiteLabel777
            ? GetResourceDataByRefIdAndResourceId("Product_Saba", productId)
            : string.Empty;

        return string.IsNullOrEmpty(productName)
            ? GetResourceDataByRefIdAndResourceId("Product", productId)
            : productName;
    }

    public static string GetProductDetailName(int productDetailId, bool isSiteLabel777)
    {
        var productDetailName = isSiteLabel777
            ? GetResourceDataByRefIdAndResourceId("ProductDetail_Saba", productDetailId)
            : string.Empty;

        return string.IsNullOrEmpty(productDetailName)
            ? GetResourceDataByRefIdAndResourceId("ProductDetail", productDetailId)
            : productDetailName;
    }

    private static string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
        => Instance.GetResourceDataByRefIdAndResourceId(refId, resourceId.ToString(), LanguageCode, LanguageCode);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}