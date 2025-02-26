using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IForecastService
{
    Task<HashSet<int>> GetBetTypeIdsSupportScoreMap();
}

public class ForecastService(IDynamicRepository repository, ICacheService cacheService) : IForecastService
{
    private const int CacheTimeInHours = 2;
    private const string CacheKey = "AlphaPlus.BetLists.Services.ForecastService.ScoreMapBetTypes";

    private readonly IDynamicRepository repository = repository;
    private readonly ICacheService cacheService = cacheService;

    public async Task<HashSet<int>> GetBetTypeIdsSupportScoreMap()
        => await cacheService.GetOrSetAsync(
        CacheKey,
        async () =>
        {
            var scoreMaps = await repository.FetchAsync<ScoreMap>(new GetScoreMapBetTypesCriteria());
            return new HashSet<int>(scoreMaps.Select(s => s.FirstId.ToListInt()).SelectMany(x => x));
        },
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(CacheTimeInHours)));
}