using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Domain.Shared;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface ILeagueService
{
    Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll, string language);

    Task<IEnumerable<League>> GetRunningLeagues(GetRunningLeaguesParam param);

    Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language);
}

public class LeagueService : ILeagueService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public LeagueService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll, string language)
        => GetLeagues(new GetSoccerLeagueCriteria(isLive, isShowAll), language);

    public Task<IEnumerable<League>> GetRunningLeagues(GetRunningLeaguesParam param)
    {
        ICriteria criteria = (UserLevel)param.CustLevel switch
        {
            UserLevel.Super => new GetRunningLeagueSuperCriteria(param.CustId, param.BetTypes),
            UserLevel.Master => new GetRunningLeagueMasterCriteria(param.CustId, param.BetTypes),
            UserLevel.Agent => new GetRunningLeagueAgentCriteria(param.CustId, param.BetTypes),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(param.CustLevel)),
        };

        return GetLeagues(criteria, param.Language);
    }

    private async Task<IEnumerable<League>> GetLeagues(ICriteria criteria, string language)
    {
        var leagues = await repository.FetchAsync<League>(criteria);
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
}