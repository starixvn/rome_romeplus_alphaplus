using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface ISoccerService
{
    Task<IEnumerable<League>> GetLeagues(bool isLive, bool isShowAll, string language);

    Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language);

    Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetResults(
        int custId,
        string matchIds,
        bool isLive);
}

public class SoccerService : ISoccerService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public SoccerService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public async Task<IEnumerable<League>> GetLeagues(bool isLive, bool isShowAll, string language)
    {
        var leagues = await repository.FetchAsync<League>(new GetSoccerLeagueCriteria(isLive, isShowAll));
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return leagues;
    }

    public async Task<IEnumerable<Match>> GetMatches(int leagueId, bool isLive, string language)
    {
        var matches = await repository.FetchAsync<Match>(new GetSoccerMatchCriteria(leagueId, isLive));
        foreach (var item in matches)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId, language);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, language);
        }

        return matches;
    }

    public Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetResults(
        int custId,
        string matchIds,
        bool isLive)
        => repository.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(new GetSoccerForecastCriteria(custId, matchIds, isLive));
}