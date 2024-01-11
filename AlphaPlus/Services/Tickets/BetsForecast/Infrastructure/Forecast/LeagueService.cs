using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Domain.Shared;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface ILeagueService
{
    Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll, string language);

    Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language);

    Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language);
}

public class LeagueService : ILeagueService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public LeagueService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public async Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll, string language)
    {
        var leagues = await repository.FetchAsync<League>(new GetSoccerLeagueCriteria(isLive, isShowAll));
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return leagues;
    }

    public async Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language)
    {
        var matches = await repository.FetchAsync<Match>(new GetSoccerMatchCriteria(leagueId, isLive));
        foreach (var item in matches)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId, language);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, language);
        }

        return matches;
    }

    public async Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language)
    {
        ICriteria criteria = (UserLevel)custLevel switch
        {
            UserLevel.Super => new GetRunningLeagueSuperCriteria(custId, betTypes),
            UserLevel.Master => new GetRunningLeagueMasterCriteria(custId, betTypes),
            UserLevel.Agent => new GetRunningLeagueAgentCriteria(custId, betTypes),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(custLevel)),
        };

        var leagues = await repository.FetchAsync<League>(criteria);
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return leagues;
    }
}