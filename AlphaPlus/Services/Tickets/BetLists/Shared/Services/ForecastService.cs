using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IForecastService
{
    Task<HashSet<int>> GetBetTypeIdsSupportScoreMap();
}

public class ForecastService(IDynamicRepository repository, IMemoryCache cacheService) : IForecastService
{
    private const int CacheTimeInHours = 2;
    private const string CacheKey = "AlphaPlus.BetLists.Services.ForecastService.ScoreMapBetTypes";

    private readonly IDynamicRepository repository = repository;
    private readonly IMemoryCache cacheService = cacheService;

    public async Task<HashSet<int>> GetBetTypeIdsSupportScoreMap()
        => await cacheService.GetOrCreateAsync(
        CacheKey,
        async cacheEntry =>
        {
            cacheEntry.SetAbsoluteExpiration(TimeSpan.FromHours(CacheTimeInHours));
            var scoreMaps = await repository.FetchAsync<ScoreMap>(new GetScoreMapBetTypesCriteria());
            return new HashSet<int>(scoreMaps.Select(s => s.FirstId.ToListInt()).SelectMany(x => x));
        });
}