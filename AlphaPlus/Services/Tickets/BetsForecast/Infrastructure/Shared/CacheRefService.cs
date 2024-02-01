using Fanex.RemotingRef;

namespace AlphaPlus.BetsForecast.Infrastructure.Shared;

public interface ICacheRefService
{
    string GetLeagueName(int leagueId, string language);

    string GetTeamName(int teamId, string language);

    string GetBetTypeName(int betType, string language);

    string GetBetTypeName(string betType, string language);
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

    public string GetBetTypeName(int betType, string language)
        => GetBetTypeName(betType.ToString(), language);

    public string GetBetTypeName(string betType, string language)
        => remotingRefService.GetBetTypeName(betType, language);
}