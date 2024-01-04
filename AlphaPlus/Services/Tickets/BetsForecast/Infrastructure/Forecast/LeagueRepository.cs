using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface ILeagueRepository
{
    Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll);

    Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive);
}

public class LeagueRepository : ILeagueRepository
{
    private readonly IDynamicRepository repository;

    public LeagueRepository(IDynamicRepository repository)
        => this.repository = repository;

    public Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll)
        => repository.FetchAsync<League>(new GetSoccerLeagueCriteria(isLive, isShowAll));

    public Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive)
        => repository.FetchAsync<Match>(new GetSoccerMatchCriteria(leagueId, isLive));
}