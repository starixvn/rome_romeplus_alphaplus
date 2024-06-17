using Fanex.RemotingRef;

namespace AlphaPlus.BetLists.Shared.Helpers;

public static class CacheRefServiceHelper
{
    internal static IRemotingRefService Instance { get; set; }

    public static string GetBetTypeName(int betTypeId)
        => Instance.GetBetTypeName(betTypeId.ToString(), LanguageCode);

    public static string GetLeagueName(int leagueId)
        => Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

    public static string GetLeagueName(int leagueId, int leagueGroupId)
        => Instance.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty);

    // public static string GetLeague3rdName(int league3rdId)
    //     => Instance.GetLeagueName3rd(league3rdId.ToString(), LanguageCode);

    // public static string GetProductName(int productId)
    //     => Instance.GetResourceDataByRefIdAndResourceId(
    //         "Product",
    //         productId.ToString(),
    //         LanguageCode,
    //         string.Empty);

    // public static string GetProductDetailName(string refId, int resourceId)
    //     => Instance.GetResourceDataByRefIdAndResourceId(
    //         refId,
    //         resourceId.ToString(),
    //         LanguageCode,
    //         string.Empty);

    public static string GetSportName(int sportTypeId)
        => Instance.GetSportName(sportTypeId.ToString(), LanguageCode, string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}