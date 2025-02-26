using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Models;
using Microsoft.Extensions.Options;

namespace AlphaPlus.Reports.Features.Shared;

public interface ILeagueService
{
    string GetLeagueName(int leagueId, byte leagueType, int leagueGroupId);
}

public class LeagueService(
    ICacheRefService cacheRefService,
    IOptions<AppSettings> options) : ILeagueService
{
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly AppSettings appSettings = options.Value;

    public string GetLeagueName(int leagueId, byte leagueType, int leagueGroupId)
    {
        if (leagueType == LeagueType.ThirdParty)
        {
            return cacheRefService.GetLeagueName3rd(leagueId);
        }

        return appSettings.IsSiteLabel777
            ? cacheRefService.GetLeagueName(leagueId, leagueGroupId)
            : cacheRefService.GetLeagueName(leagueId);
    }
}
