using Microsoft.Extensions.Options;
using AlphaPlus.Reports.Features.Shared.Models;
using Fanex.Data.Repository;
using AlphaPlus.PartialUM.Services;
using AlphaPlus.Reports.Features.Shared;
using Fanex.Caching;

namespace AlphaPlus.Reports.Features.EnteredResult;

public interface IEnteredResultService
{
    Task<EnteredResultSport> GetTotalEnteredResults(string siteName);

    EnteredResultSport GetTotalEnteredResults(IEnumerable<EnteredResultSport> dailyEnteredSports);

    Task<IEnumerable<EnteredResultSport>> GetEnteredResults(string siteName);
}

public class EnteredResultService(
    IDynamicRepository repository,
    ICacheService cacheService,
    IPartialUMService partialUMService,
    ICacheRefService cacheRefService,
    IOptions<AppSettings> options) : IEnteredResultService
{
    private const int CacheInMinutes = 1;
    private const string CacheKey = "AlphaPlus.Reports.EnteredResult";

    private readonly IDynamicRepository repository = repository;
    private readonly ICacheService cacheService = cacheService;
    private readonly IPartialUMService partialUMService = partialUMService;
    private readonly ICacheRefService cacheRefService = cacheRefService;
    private readonly AppSettings appSettings = options.Value;

    public async Task<EnteredResultSport> GetTotalEnteredResults(string siteName)
        => GetTotalEnteredResults(await GetEnteredResults(siteName));

    public EnteredResultSport GetTotalEnteredResults(IEnumerable<EnteredResultSport> dailyEnteredSports)
    {
        var totalEnteredToday = 0;
        var totalNonEnteredToday = 0;
        var totalEnteredYesterday = 0;
        var totalNonEnteredYesterday = 0;
        var today = SystemTime.Today;
        var yesterday = SystemTime.Yesterday;

        foreach (var data in dailyEnteredSports)
        {
            if (data.EnteredResultToday != null)
            {
                totalEnteredToday += data.EnteredResultToday.Entered;
                totalNonEnteredToday += data.EnteredResultToday.NonEntered;
                today = data.EnteredResultToday.EventDate;
            }

            if (data.EnteredResultYesterday != null)
            {
                totalEnteredYesterday += data.EnteredResultYesterday.Entered;
                totalNonEnteredYesterday += data.EnteredResultYesterday.NonEntered;
                yesterday = data.EnteredResultYesterday.EventDate;
            }
        }

        var totalToday = new EnteredResultEntity(today, totalEnteredToday, totalNonEnteredToday);
        var totalYesterday = new EnteredResultEntity(yesterday, totalEnteredYesterday, totalNonEnteredYesterday);

        return new EnteredResultSport(0, null, totalToday, totalYesterday);
    }

    public Task<IEnumerable<EnteredResultSport>> GetEnteredResults(string siteName)
        => cacheService.GetOrSetAsync(
        CacheKey,
        () => GetEnteredResultsFromDB(siteName),
        new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(CacheInMinutes)));

    internal async Task<IEnumerable<EnteredResultSport>> GetEnteredResultsFromDB(string siteName)
    {
        var criteria = new EnteredResultCriteria(appSettings.LocalTest);
        await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

        var dailyEnteredSports = await repository.FetchAsync<EnteredResultEntity>(criteria);

        if (dailyEnteredSports.Any())
        {
            return dailyEnteredSports.GroupBy(
                dailyEnteredSport => dailyEnteredSport.SportType,
                (sportType, data) => new EnteredResultSport(
                    sportType,
                    cacheRefService.GetSportName(sportType),
                    data.FirstOrDefault(enteredSportResult => enteredSportResult.IsToday),
                    data.FirstOrDefault(enteredSportResult => !enteredSportResult.IsToday)));
        }

        return [];
    }
}
