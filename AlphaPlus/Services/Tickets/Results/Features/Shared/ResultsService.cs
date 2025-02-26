using AlphaPlus.PartialUM.Services;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Results.Features.Shared;

public interface IResultsService
{
    Task<IEnumerable<Sport>> GetSports(SportParam param);

    Task<IEnumerable<League>> GetLeagues(LeagueParam param);

    Task<DateTime> GetBalanceUpDate();

    Task<DateTime> GetMoveBetsDate();

    Task<BalanceUpAndMoveBetsDate> GetBalanceUpAndMoveBetsDate();
}

public class ResultsService(
    IDynamicRepository repository,
    ICacheService cacheService,
    IPartialUMService partialUMService) : IResultsService
{
    private const int BalanceUpAndMoveBetsDateCacheInMinutes = 2;
    private const string BalanceUpAndMoveBetsDateCacheKey = "AlphaPlus.Results.Shared.BalanceUpAndMoveBetsDate";

    public async Task<IEnumerable<Sport>> GetSports(SportParam param)
    {
        var moveBetsDay = await GetMoveBetsDate();
        var criteria = new GetResultSportsByDateCriteria(param.EventDate, moveBetsDay, param.IsShowAll);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        var sports = (await repository.FetchAsync<Sport>(criteria)).ToList();

        if (sports.Any())
        {
            sports.RemoveAll(sport => param.SportsStatus.TryGetValue(sport.Id, out var status) && !status);
        }

        return sports;
    }

    public async Task<IEnumerable<League>> GetLeagues(LeagueParam param)
    {
        var moveBetsDay = await GetMoveBetsDate();
        var criteria = new GetResultLeaguesByDateCriteria(param.EventDate, moveBetsDay, param.IsShowAll, param.SportType);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), param.SiteName);

        return await repository.FetchAsync<League>(criteria);
    }

    public async Task<DateTime> GetBalanceUpDate() => (await GetBalanceUpAndMoveBetsDate()).BalanceUpDay;

    public async Task<DateTime> GetMoveBetsDate() => (await GetBalanceUpAndMoveBetsDate()).MoveBetsDay;

    public Task<BalanceUpAndMoveBetsDate> GetBalanceUpAndMoveBetsDate()
        => cacheService.GetOrSetAsync(
        BalanceUpAndMoveBetsDateCacheKey,
        () => repository.GetAsync<BalanceUpAndMoveBetsDate>(new GetBalanceUpAndMoveBetsDayCriteria()),
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(BalanceUpAndMoveBetsDateCacheInMinutes)));
}