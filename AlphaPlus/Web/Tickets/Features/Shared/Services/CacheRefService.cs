using Fanex.RemotingRef;

namespace AlphaPlus.Tickets.Site.Features.Shared.Services;

public interface ICacheRefService
{
    string GetLeagueName(int leagueId);

    string GetTeamName(int teamId);
}

public class CacheRefService : ICacheRefService
{
    private readonly IRemotingRefService remotingRefService;

    public CacheRefService(IRemotingRefService remotingRefService)
        => this.remotingRefService = remotingRefService;

    public string GetLeagueName(int leagueId)
        => remotingRefService.GetLeagueName(leagueId.ToString(), LanguageCode, string.Empty);

    public string GetTeamName(int teamId)
        => remotingRefService.GetTeamName(teamId.ToString(), LanguageCode, string.Empty);

    private static string LanguageCode => Thread.CurrentThread.CurrentCulture.Name;
}