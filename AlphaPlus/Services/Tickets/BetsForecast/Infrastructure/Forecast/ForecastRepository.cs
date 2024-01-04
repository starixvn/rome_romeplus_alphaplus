using AlphaPlus.BetsForecast.Domain.Forecast;
using AlphaPlus.BetsForecast.Infrastructure.Forecast.Criteria;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.Forecast;

public interface IForecastRepository
{
    Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetSoccer(
        int custId,
        string matchIds,
        bool isLive);
}

public class ForecastRepository : IForecastRepository
{
    private readonly IDynamicRepository repository;

    public ForecastRepository(IDynamicRepository repository)
        => this.repository = repository;

    public Task<(IEnumerable<SoccerMatch>, IEnumerable<SoccerBetType>, IEnumerable<SoccerWinLoss>)> GetSoccer(
        int custId,
        string matchIds,
        bool isLive)
        => repository.FetchMultipleAsync<SoccerMatch, SoccerBetType, SoccerWinLoss>(new GetFCSoccerCriteria(custId, matchIds, isLive));
}