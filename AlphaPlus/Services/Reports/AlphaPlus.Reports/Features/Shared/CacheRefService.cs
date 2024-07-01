using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.RemotingRef;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Features.Shared;

public interface ICacheRefService
{
    string GetSportName(int sportType);

    string GetBetTypeName(int betType);

    string GetLeagueName(int leagueId);

    string GetLeagueName(int leagueId, int leagueGroupId);

    string GetLeagueName3rd(int league3rdId);

    string GetTeamName(int teamId);

    string GetTeamName3rd(int team3rdId);

    string GetOddsTypeName(string oddsTypeId);

    string GetProductName(int productId);

    string GetProductName(string productId);

    string GetProductDetailName(int productDetailId);

    string GetResourceDataByRefIdAndResourceId(string refId, int resourceId);
}

public class CacheRefService(
    IRemotingRefService remotingRefService,
    IOptions<AppSettings> options) : ICacheRefService
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

    public string GetTeamName3rd(int team3rdId)
        => remotingRefService.GetTeamName3rd(team3rdId.ToString(), LanguageCode);

    public string GetOddsTypeName(string oddsTypeId)
        => remotingRefService.GetOddsTypeName(oddsTypeId, LanguageCode);

    public string GetProductName(int productId)
        => GetProductName(productId.ToString());

    public string GetProductName(string productId)
    {
        var productName = options.Value.IsSiteLabel777
            ? GetResourceDataByRefIdAndResourceId("Product_Saba", productId)
            : string.Empty;

        return string.IsNullOrEmpty(productName)
            ? GetResourceDataByRefIdAndResourceId("Product", productId)
            : productName;
    }

    public string GetProductDetailName(int productDetailId)
    {
        var productDetailName = options.Value.IsSiteLabel777
            ? GetResourceDataByRefIdAndResourceId("ProductDetail_Saba", productDetailId)
            : string.Empty;

        return string.IsNullOrEmpty(productDetailName)
            ? GetResourceDataByRefIdAndResourceId("ProductDetail", productDetailId)
            : productDetailName;
    }

    public string GetResourceDataByRefIdAndResourceId(string refId, int resourceId)
        => GetResourceDataByRefIdAndResourceId(refId, resourceId.ToString());

    private string GetResourceDataByRefIdAndResourceId(string refId, string resourceId)
        => remotingRefService.GetResourceDataByRefIdAndResourceId(
        refId,
        resourceId,
        LanguageCode,
        string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}