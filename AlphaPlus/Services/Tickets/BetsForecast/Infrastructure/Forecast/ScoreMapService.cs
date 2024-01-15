using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Domain.Shared;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface IScoreMapService
{
    Task<IEnumerable<ScoreMapBetType>[]> GetBetTypes(string language);

    Task<IEnumerable<League>> GetLeagues(int custId, string language);

    Task<IEnumerable<Match>> GetMatches(GetScoreMapMatchesParam param);

    Task<IEnumerable<ScoreMapResultDTO>> GetResults(GetScoreMapResultParam param);
}

public class ScoreMapService : IScoreMapService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public ScoreMapService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public async Task<IEnumerable<ScoreMapBetType>[]> GetBetTypes(string language)
    {
        var liveBetTypes = new List<ScoreMapBetType>();
        var deadBetTypes = new List<ScoreMapBetType>();
        var rawdata = await repository.FetchAsync<ScoreMapBetTypeEntity>(new GetScoreMapBetTypeCriteria());
        foreach (var item in rawdata)
        {
            var betType = new ScoreMapBetType(
                item.TypeId,
                item.TypeId == 5 ? null : cacheRefService.GetBetTypeName(item.TypeId, language),
                item.FirstId,
                item.InfoId.Split(',').Select(id => cacheRefService.GetBetTypeName(id, language)));

            if (item.EvenType)
            {
                liveBetTypes.Add(betType);
            }
            else
            {
                deadBetTypes.Add(betType);
            }
        }

        return new[] { liveBetTypes, deadBetTypes };
    }

    public async Task<IEnumerable<League>> GetLeagues(int custId, string language)
    {
        var leagues = await repository.FetchAsync<League>(new GetScoreMapLeagueCriteria(custId));
        foreach (var item in leagues)
        {
            item.LeagueName = cacheRefService.GetLeagueName(item.LeagueId, language);
        }

        return leagues;
    }

    public async Task<IEnumerable<Match>> GetMatches(GetScoreMapMatchesParam param)
    {
        var matches = await repository.FetchAsync<Match>(new GetScoreMapMatchCriteria(param.CustId, param.LeagueId));
        foreach (var item in matches)
        {
            item.HomeName = cacheRefService.GetTeamName(item.HomeId, param.Language);
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, param.Language);
        }

        return matches;
    }

    public async Task<IEnumerable<ScoreMapResultDTO>> GetResults(GetScoreMapResultParam param)
    {
        ICriteria criteria = (UserLevel)param.CustLevel switch
        {
            UserLevel.Super => new GetScoreMapResultSuperCriteria(param.CustId, param.MatchIds, param.LiveBetTypes, param.DeadBetTypes),
            UserLevel.Master => new GetScoreMapResultMasterCriteria(param.CustId, param.MatchIds, param.LiveBetTypes, param.DeadBetTypes),
            UserLevel.Agent => new GetScoreMapResultAgentCriteria(param.CustId, param.MatchIds, param.LiveBetTypes, param.DeadBetTypes),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(param.CustLevel)),
        };

        var (scoreMapResultEntities, matches) = await repository.FetchMultipleAsync<ScoreMapResultEntity, Match>(criteria);
        var matchHash = matches.ToDictionary(kvp => kvp.MatchId, kvp => kvp);

        return scoreMapResultEntities.GroupBy(
            s => s.MatchId,
            (matchId, entities) =>
            {
                if (!matchHash.TryGetValue(matchId, out var match))
                {
                    match = new Match();
                }

                return new ScoreMapResultDTO(entities)
                {
                    MatchId = matchId,
                    HomeId = match.HomeId,
                    HomeName = cacheRefService.GetTeamName(match.HomeId, param.Language),
                    AwayId = match.AwayId,
                    AwayName = cacheRefService.GetTeamName(match.AwayId, param.Language)
                };
            });
    }
}