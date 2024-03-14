using Fanex.RemotingRef;

namespace AlphaPlus.Reports.Features.Shared;

public interface ICacheRefService
{
    string GetSportName(int sportType);

    string GetBetTypeName(int betType);

    string GetLeagueName(int leagueId);

    string GetLeagueName(int leagueId, int leagueGroupId);

    string GetLeagueName3rd(int league3rdId);

    string GetTeamName(int teamId);

    string GetOddsTypeName(string oddsTypeId);

    string GetProductName(int productId);

    string GetProductDetailName(int resourceId);

    string GetResourceDataByRefIdAndResourceId(string refId, int resourceId);
}

public class CacheRefService(IRemotingRefService remotingRefService) : ICacheRefService
{
    public string GetSportName(int sportType)
        => remotingRefService.GetSportName(sportType.ToString(), LanguageCode);

    public string GetBetTypeName(int betType)
        => remotingRefService.GetBetTypeName(betType.ToString(), LanguageCode);

    public string GetLeagueName(int leagueId)
        => remotingRefService.GetLeagueName(leagueId.ToString(), LanguageCode);

    public string GetLeagueName(int leagueId, int leagueGroupId)
        => remotingRefService.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty);

    public string GetLeagueName3rd(int league3rdId)
        => remotingRefService.GetLeagueName3rd(league3rdId.ToString(), LanguageCode);

    public string GetTeamName(int teamId)
        => remotingRefService.GetTeamName(teamId.ToString(), LanguageCode);

    public string GetOddsTypeName(string oddsTypeId)
        => remotingRefService.GetOddsTypeName(oddsTypeId, LanguageCode);

    public string GetProductName(int productId)
        => GetResourceDataByRefIdAndResourceId("Product", productId);

    public string GetProductDetailName(int resourceId)
        => GetResourceDataByRefIdAndResourceId("ProductDetail", resourceId);

    public string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
        => remotingRefService.GetResourceDataByRefIdAndResourceId(
        refId,
        resourceId.ToString(),
        LanguageCode,
        string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}