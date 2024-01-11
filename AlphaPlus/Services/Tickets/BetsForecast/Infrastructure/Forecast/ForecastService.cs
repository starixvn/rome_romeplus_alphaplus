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
}
