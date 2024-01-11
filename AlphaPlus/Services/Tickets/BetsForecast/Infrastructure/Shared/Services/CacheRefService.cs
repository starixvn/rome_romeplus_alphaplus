using Fanex.RemotingRef;

namespace AlphaPlus.BetsForecast.Infrastructure.Shared.Services;

public interface ICacheRefService
{
    string GetLeagueName(int leagueId, string language);

    string GetTeamName(int teamId, string language);
}

public class CacheRefService : ICacheRefService
{
    private readonly IRemotingRefService remotingRefService;

    public CacheRefService(IRemotingRefService remotingRefService)
        => this.remotingRefService = remotingRefService;

    public string GetLeagueName(int leagueId, string language)
        => remotingRefService.GetLeagueName(leagueId.ToString(), language);

    public string GetTeamName(int teamId, string language)
        => remotingRefService.GetTeamName(teamId.ToString(), language);
}