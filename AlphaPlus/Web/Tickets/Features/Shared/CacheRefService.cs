using AlphaPlus.Tickets.Site.Features.Shared.Models;
using Fanex.RemotingRef;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Tickets.Site.Features.Shared;

public interface ICacheRefService
{
    string GetSportName(int sportTypeId);

    string GetBetTypeName(int betTypeId);

    string GetLeagueName(int leagueId);

    string GetLeagueName(int leagueId, int leagueGroupId);

    string GetLeague3rdName(int league3rdId);

    string GetProductName(int productId);
}

public class CacheRefService(
    IRemotingRefService remotingRefService,
    IOptions<AppSettings> options) : ICacheRefService
{
    public string GetSportName(int sportTypeId)
        => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode);

    public string GetBetTypeName(int betTypeId)
        => remotingRefService.GetBetTypeName(betTypeId.ToString(), LanguageCode);

    public string GetLeagueName(int leagueId)
        => remotingRefService.GetLeagueName(leagueId.ToString(), LanguageCode);

    public string GetLeagueName(int leagueId, int leagueGroupId)
        => remotingRefService.GetLeagueName(leagueId.ToString(), leagueGroupId.ToString(), LanguageCode, string.Empty);

    public string GetLeague3rdName(int league3rdId)
        => remotingRefService.GetLeagueName3rd(league3rdId.ToString(), LanguageCode);

    public string GetProductName(int productId) => GetProductName(productId.ToString());

    private string GetProductName(string productId)
    {
        var productName = options.Value.IsSiteLabel777
            ? GetResourceDataByRefIdAndResourceId("Product_Saba", productId)
            : string.Empty;

        return string.IsNullOrEmpty(productName)
            ? GetResourceDataByRefIdAndResourceId("Product", productId)
            : productName;
    }

    private string GetResourceDataByRefIdAndResourceId(string refId, string resourceId)
        => remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, LanguageCode, string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}