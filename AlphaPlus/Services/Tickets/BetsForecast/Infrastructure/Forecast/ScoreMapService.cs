using System.Runtime.CompilerServices;
using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Params;
using AlphaPlus.BetsForecast.Infrastructure.Shared;
using Fanex.Data.Repository;
using Microsoft.VisualBasic;

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
                item.TypeId == BetTypeConstants._1X2 ? null : cacheRefService.GetBetTypeName(item.TypeId, language),
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
        var (scoreMapResultEntities, matches) = await repository.FetchMultipleAsync<ScoreMapResultEntity, Match>(GetResultsCriteria(param));
        var matchHash = matches.ToDictionary(kvp => kvp.MatchId, kvp => kvp);

        var resultHash = scoreMapResultEntities.GroupBy(
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
            }).ToDictionary(kvp => kvp.MatchId, kvp => kvp);
        AddEmptyResults(resultHash, matchHash, param.Language);

        return resultHash.Values;
    }

    private static ICriteria GetResultsCriteria(GetScoreMapResultParam param)
    {
        var custLevel = Enumeration.FromValue<UserLevel>(param.CustLevel);
        if (custLevel.IsSuperLevel())
        {
            return new GetScoreMapResultSuperCriteria(param.CustId, param.MatchIds, param.LiveBetTypes, param.DeadBetTypes);
        }

        if (custLevel.IsMasterLevel())
        {
            return new GetScoreMapResultMasterCriteria(param.CustId, param.MatchIds, param.LiveBetTypes, param.DeadBetTypes);
        }

        return new GetScoreMapResultAgentCriteria(param.CustId, param.MatchIds, param.LiveBetTypes, param.DeadBetTypes);
    }

    private void AddEmptyResults(
        IDictionary<int, ScoreMapResultDTO> resultHash,
        IDictionary<int, Match> matchHash,
        string language)
    {
        foreach (KeyValuePair<int, Match> item in matchHash)
        {
            if (!resultHash.ContainsKey(item.Key))
            {
                var entity = new ScoreMapResultEntity
                {
                    MatchId = item.Key,
                    Home = item.Value.HomeId,
                    Away = item.Value.AwayId,
                    HT = null,
                    FT = null,
                };
                resultHash.Add(item.Key, new ScoreMapResultDTO([entity])
                {
                    MatchId = item.Key,
                    HomeId = item.Value.HomeId,
                    HomeName = cacheRefService.GetTeamName(item.Value.HomeId, language),
                    AwayId = item.Value.AwayId,
                    AwayName = cacheRefService.GetTeamName(item.Value.AwayId, language)
                });
            }
        }
    }
}