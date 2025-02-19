using AlphaPlus.BetLists.Shared.Criteria;
using AlphaPlus.BetLists.Shared.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.BetLists.Shared.Services;

public interface IReportDateService
{
    Task<DateTime> GetBalanceUpDay();

    Task<DateTime> GetMoveBetsDay();

    Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay();
}

public class ReportDateService(IDynamicRepository repository, IMemoryCache cacheService) : IReportDateService
{
    private const string BalanceUpDayAndMoveBetsDayCacheKey = "AlphaPlus.BetLists.BalanceUpDayAndMoveBetsDay";
    private const int CacheBalanceUpDayAndMoveBetsDayInMinutes = 2;

    private readonly IDynamicRepository repository = repository;
    private readonly IMemoryCache cacheService = cacheService;

    public static IReportDateService Instance { get; set; }

    public async Task<DateTime> GetBalanceUpDay() => (await GetBalanceUpDayAndMoveBetsDay()).BalanceUpDay;

    public async Task<DateTime> GetMoveBetsDay() => (await GetBalanceUpDayAndMoveBetsDay()).MoveBetsDay;

    public Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay()
        => cacheService.GetOrCreateAsync(
            BalanceUpDayAndMoveBetsDayCacheKey,
            cacheEntry =>
            {
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheBalanceUpDayAndMoveBetsDayInMinutes));
                return repository.GetAsync<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria());
            });
}