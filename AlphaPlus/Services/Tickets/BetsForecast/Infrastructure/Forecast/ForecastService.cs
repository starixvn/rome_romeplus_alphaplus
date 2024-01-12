using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Domain.Shared;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Helper;
using AlphaPlus.BetsForecast.Infrastructure.Shared.Services;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface IForecastService
{
    Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetSoccer(
        int custId,
        string matchIds,
        bool isLive);

    Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(byte custLevel, int custId, int leagueId, string language);

    Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int leagueId, string language);

    Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int matchId, int betType, string language);

    Task<IEnumerable<ScoreMapBetType>[]> GetScoreMapBetTypes(string language);

    Task<IEnumerable<ScoreMapResult>> GetScoreMapResult(
        byte custLevel,
        int custId,
        string matchIds,
        string liveBetTypes,
        string deadBetTypes,
        string language);
}

public class ForecastService : IForecastService
{
    private readonly IDynamicRepository repository;
    private readonly ICacheRefService cacheRefService;

    public ForecastService(IDynamicRepository repository, ICacheRefService cacheRefService)
        => (this.repository, this.cacheRefService) = (repository, cacheRefService);

    public Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetSoccer(
        int custId,
        string matchIds,
        bool isLive)
        => repository.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(new GetFCSoccerCriteria(custId, matchIds, isLive));

    public Task<IEnumerable<ForecastHDPOULive>> GetHDPOULive(byte custLevel, int custId, int leagueId, string language)
    {
        var langCode = LanguageHelper.GetLangCode(language);
        ICriteria criteria = (UserLevel)custLevel switch
        {
            UserLevel.Super => new GetHDPOULiveSuperCriteria(custId, leagueId, langCode),
            UserLevel.Master => new GetHDPOULiveMasterCriteria(custId, leagueId, langCode),
            UserLevel.Agent => new GetHDPOULiveAgentCriteria(custId, leagueId, langCode),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(custLevel)),
        };

        return GetForecast<ForecastHDPOULive>(criteria, language);
    }

    public Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int leagueId, string language)
        => GetForecast<Forecast1X2>(new Get1X2ByLeagueCriteria(custLevel, custId, leagueId), language);

    public Task<IEnumerable<Forecast1X2>> Get1X2(byte custLevel, int custId, int matchId, int betType, string language)
        => GetForecast<Forecast1X2>(new Get1X2ByMatchCriteria(custLevel, custId, matchId, betType), language);

    private async Task<IEnumerable<T>> GetForecast<T>(ICriteria criteria, string language) where T : ForecastMatch
    {
        var rawdata = await repository.FetchAsync<T>(criteria);
        foreach (var item in rawdata)
        {
            item.HomeName = $"{cacheRefService.GetTeamName(item.HomeId, language)}{(item.IsNeutral ? " (N)" : string.Empty)}";
            item.AwayName = cacheRefService.GetTeamName(item.AwayId, language);
        }

        return rawdata;
    }

    public async Task<IEnumerable<ScoreMapBetType>[]> GetScoreMapBetTypes(string language)
    {
        IList<ScoreMapBetType> liveBetTypes = new List<ScoreMapBetType>();
        IList<ScoreMapBetType> deadBetTypes = new List<ScoreMapBetType>();
        var rawdata = await repository.FetchAsync<ScoreMapBetTypeEntity>(new GetScoreMapBetTypeCriteria());
        foreach (var item in rawdata)
        {
            var scoreMapBetType = new ScoreMapBetType(
                item.TypeId,
                item.TypeId == 5 ? null : cacheRefService.GetBetTypeName(item.TypeId, language),
                item.FirstId);

            if (item.EvenType)
            {
                liveBetTypes.Add(scoreMapBetType);
            }
            else
            {
                deadBetTypes.Add(scoreMapBetType);
            }
        }

        return new[] { liveBetTypes, deadBetTypes };
    }

    public async Task<IEnumerable<ScoreMapResult>> GetScoreMapResult(
        byte custLevel,
        int custId,
        string matchIds,
        string liveBetTypes,
        string deadBetTypes,
        string language)
    {
        const int DefaultRowColumn = 7;
        ICriteria criteria = (UserLevel)custLevel switch
        {
            UserLevel.Super => new GetScoreMapResultSuperCriteria(custId, matchIds, liveBetTypes, deadBetTypes),
            UserLevel.Master => new GetScoreMapResultMasterCriteria(custId, matchIds, liveBetTypes, deadBetTypes),
            UserLevel.Agent => new GetScoreMapResultAgentCriteria(custId, matchIds, liveBetTypes, deadBetTypes),
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(custLevel)),
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

                var scoreMap = new ScoreMapResult(DefaultRowColumn, DefaultRowColumn)
                {
                    MatchId = matchId,
                    HomeId = match.HomeId,
                    HomeName = cacheRefService.GetTeamName(match.HomeId, language),
                    AwayId = match.AwayId,
                    AwayName = cacheRefService.GetTeamName(match.AwayId, language)
                };

                foreach (var item in entities)
                {
                    if (item.HT.HasValue)
                    {
                        scoreMap.HaftTime[item.Away, item.Home] = (int)Math.Round(item.HT.Value, MidpointRounding.AwayFromZero);
                    }

                    if (item.FT.HasValue)
                    {
                        scoreMap.FullTime[item.Away, item.Home] = (int)Math.Round(item.FT.Value, MidpointRounding.AwayFromZero);
                    }
                }

                return scoreMap;
            });
    }
}
