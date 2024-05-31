using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IForecastService
{
    Task<IEnumerable<int>> GetBetTypeIdsSupportScoreMap();
}

public class ForecastService : IForecastService
{
    private const int CacheTimeInHours = 2;
    private const string CacheKey = "AlphaPlus.BetLists.Services.ForecastService.ScoreMapBetTypes";
    private readonly IDynamicRepository repository;
    private readonly ICacheService cacheService;

    public ForecastService(IDynamicRepository repository, ICacheService cacheService)
        => (this.repository, this.cacheService) = (repository, cacheService);

    public async Task<IEnumerable<int>> GetBetTypeIdsSupportScoreMap()
        => (await GetScoreMaps())
        .Select(s => s.FirstId.ToListInt())
        .SelectMany(x => x)
        .Distinct()
        .ToList();

    private Task<IEnumerable<ScoreMap>> GetScoreMaps()
        => cacheService.GetOrSetAsync(
        CacheKey,
        () => repository.FetchAsync<ScoreMap>(new GetScoreMapBetTypesCriteria()),
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(CacheTimeInHours)));
}