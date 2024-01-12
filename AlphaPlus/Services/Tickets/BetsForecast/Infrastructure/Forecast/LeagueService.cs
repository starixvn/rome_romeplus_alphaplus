using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Domain.Shared;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface ILeagueService
{
    Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll, string language);

    Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language);

    Task<IEnumerable<League>> GetScoreMapLeagues(int custId, string language);

    Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language);

    Task<IEnumerable<Match>> GetScoreMapMatches(int custId, int leagueId, string language);

    Task<ForecastMatch> GetMatch(int matchId, int betType, string language);
}

public class LeagueService : ILeagueService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public LeagueService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public Task<IEnumerable<League>> GetSoccerLeagues(bool isLive, bool isShowAll, string language)
        => GetLeagues(new GetSoccerLeagueCriteria(isLive, isShowAll), language);

    public Task<IEnumerable<League>> GetRunningLeagues(byte custLevel, int custId, string betTypes, string language)
    {
        ICriteria criteria = (UserLevel)custLevel switch
        {
            UserLevel.Super => new GetRunningLeagueSuperCriteria(custId, betTypes),
            UserLevel.Master => new GetRunningLeagueMasterCriteria(custId, betTypes),
            UserLevel.Agent => new GetRunningLeagueAgentCriteria(custId, betTypes),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(custLevel)),
        };

        return GetLeagues(criteria, language);
    }

    public Task<IEnumerable<League>> GetScoreMapLeagues(int custId, string language)
        => GetLeagues(new GetScoreMapLeagueCriteria(custId), language);

    private async Task<IEnumerable<League>> GetLeagues(ICriteria criteria, string language)
    {
        var leagues = await repository.FetchAsync<League>(criteria);
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return leagues;
    }

    public Task<IEnumerable<Match>> GetSoccerMatches(int leagueId, bool isLive, string language)
        => GetMatches(new GetSoccerMatchCriteria(leagueId, isLive), language);

    public Task<IEnumerable<Match>> GetScoreMapMatches(int custId, int leagueId, string language)
        => GetMatches(new GetScoreMapMatchCriteria(custId, leagueId), language);

    private async Task<IEnumerable<Match>> GetMatches(ICriteria criteria, string language)
    {
        var matches = await repository.FetchAsync<Match>(criteria);
        foreach (var item in matches)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId, language);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, language);
        }

        return matches;
    }

    public async Task<ForecastMatch> GetMatch(int matchId, int betType, string language)
    {
        var match = await repository.GetAsync<ForecastMatch>(new GetMatchCriteria(matchId, betType));
        if (match != null)
        {
            match.HomeName = cacheRefService.GetTeamName(match.HomeId, language);
            match.AwayName = cacheRefService.GetTeamName(match.AwayId, language);
        }

        return match;
    }
}