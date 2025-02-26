using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IReportDateService
{
    Task<DateTime> GetBalanceUpDay();

    Task<DateTime> GetMoveBetsDay();

    Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay();
}

public class ReportDateService(IDynamicRepository repository, ICacheService cacheService) : IReportDateService
{
    private const string BalanceUpDayAndMoveBetsDayCacheKey = "AlphaPlus.BetLists.BalanceUpDayAndMoveBetsDay";
    private const int CacheBalanceUpDayAndMoveBetsDayInMinutes = 2;

    private readonly IDynamicRepository repository = repository;
    private readonly ICacheService cacheService = cacheService;

    public static IReportDateService Instance { get; set; }

    public async Task<DateTime> GetBalanceUpDay() => (await GetBalanceUpDayAndMoveBetsDay()).BalanceUpDay;

    public async Task<DateTime> GetMoveBetsDay() => (await GetBalanceUpDayAndMoveBetsDay()).MoveBetsDay;

    public Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay()
        => cacheService.GetOrSetAsync(
            BalanceUpDayAndMoveBetsDayCacheKey,
            () => repository.GetAsync<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheBalanceUpDayAndMoveBetsDayInMinutes)));
}