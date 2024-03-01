using AlphaPlus.Reports.Features.Shared.Criteria;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Reports.Features.Shared;

public interface IReportDateService
{
    Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay();

    Task<DateTime> GetBalanceUpDay();

    Task<DateTime> GetMoveBetsDay();

    Task<IEnumerable<TimeRange>> GetTimeRangesToGetReportDataBetweenTables(DateTime fromDate, DateTime toDate);

    Task<string> GetStoreMappingSuffix(DateTime fromDate);
}

public class ReportDateService(IDynamicRepository repository, ICacheService cacheService) : IReportDateService
{
    private const int CacheBalanceUpDayAndMoveBetsDayInMinutes = 2;
    public const string BetTrans14SPSuffix = "_14";
    public const string WarehouseSPSuffix = "_WH";

    private const string BalanceUpDayAndMoveBetsDayCacheKey = "AlphaPlus.Reports.BalanceUpDayAndMoveBetsDay";

    public Task<BalanceUpDayAndMoveBetsDay> GetBalanceUpDayAndMoveBetsDay()
        => cacheService.GetOrSetAsync(
            BalanceUpDayAndMoveBetsDayCacheKey,
            () => repository.GetAsync<BalanceUpDayAndMoveBetsDay>(new GetBalanceUpDayAndMoveBetsDayCriteria()),
            new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheBalanceUpDayAndMoveBetsDayInMinutes)));

    public async Task<DateTime> GetBalanceUpDay() => (await GetBalanceUpDayAndMoveBetsDay()).BalanceUpDay;

    public async Task<DateTime> GetMoveBetsDay() => (await GetBalanceUpDayAndMoveBetsDay()).MoveBetsDay;

    public async Task<IEnumerable<TimeRange>> GetTimeRangesToGetReportDataBetweenTables(DateTime fromDate, DateTime toDate)
    {
        var timeRanges = new List<TimeRange>();
        var balanceUpDayAndMoveBetsDay = await GetBalanceUpDayAndMoveBetsDay();
        var eventDates = new[]
        {
            balanceUpDayAndMoveBetsDay.BalanceUpDay.Yesterday(),
            balanceUpDayAndMoveBetsDay.MoveBetsDay
        };

        foreach (var eventDate in eventDates)
        {
            if (fromDate <= eventDate && toDate <= eventDate)
            {
                // No need to cut range any more
                break;
            }

            if (fromDate <= eventDate)
            {
                // Data between 2+ places
                // Cut range: [From, Event Date][Event Date + 1, ToDate]
                timeRanges.Add(new TimeRange(fromDate, eventDate));
                fromDate = eventDate.AddDays(1);
            }
        }

        timeRanges.Add(new TimeRange(fromDate, toDate));

        return timeRanges;
    }

    public async Task<string> GetStoreMappingSuffix(DateTime fromDate)
    {
        var balanceUpDayAndMoveBetsDay = await GetBalanceUpDayAndMoveBetsDay();
        return fromDate > balanceUpDayAndMoveBetsDay.MoveBetsDay
            ? string.Empty
            : fromDate >= balanceUpDayAndMoveBetsDay.BalanceUpDay
                ? BetTrans14SPSuffix
                : WarehouseSPSuffix;
    }
}