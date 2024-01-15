using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
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
        ICriteria criteria;
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevel);
        if (custLevel.IsSuperLevel())
        {
            criteria = new GetRunningLeagueSuperCriteria(param.CustId, param.BetTypes);
        }
        else if (custLevel.IsMasterLevel())
        {
            criteria = new GetRunningLeagueMasterCriteria(param.CustId, param.BetTypes);
        }
        else
        {
            criteria = new GetRunningLeagueAgentCriteria(param.CustId, param.BetTypes);
        }

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