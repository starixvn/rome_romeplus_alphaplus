using Fanex.RemotingRef;

namespace AlphaPlus.Tickets.Site.Features.Shared;

public static class CacheRefServiceHelper
{
    private static IRemotingRefService remotingRefService;

    public static string GetBetTypeName(int betTypeId)
        => Instance.GetBetTypeName(betTypeId.ToString(), LanguageCode);

    public static string GetLeagueName(int leagueId)
        => Instance.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

    public static string GetLeagueName(int leagueId, int leagueGroupId)
        => remotingRefService.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty);

    public static string GetLeague3rdName(int league3rdId)
        => Instance.GetLeagueName3rd(league3rdId.ToString(), LanguageCode);

    public static string GetProductName(int productId)
        => Instance.GetResourceDataByRefIdAndResourceId(
            "Product",
            productId.ToString(),
            LanguageCode,
            string.Empty);

    public static string GetProductDetailName(string refId, int resourceId)
        => Instance.GetResourceDataByRefIdAndResourceId(
            refId,
            resourceId.ToString(),
            LanguageCode,
            string.Empty);

    public static string GetSportName(int sportTypeId)
        => Instance.GetSportName(sportTypeId.ToString(), LanguageCode, string.Empty);

    internal static IRemotingRefService Instance
    {
        get
        {
            remotingRefService = remotingRefService ?? new RemotingRefService();

            return remotingRefService;
        }
    }

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}