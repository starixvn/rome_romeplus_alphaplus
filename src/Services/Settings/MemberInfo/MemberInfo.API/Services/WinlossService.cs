using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLoss;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    public interface IWinlossService
    {
        Task<EnteredSportResult> GetEnteredSportResults();

        Task<EnteredSportResult> GetEnteredSportResultsAsync();
    }

    public class WinlossService : IWinlossService
    {
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";
        private readonly IDynamicRepository dynamicRepository;
        private readonly IMemoryCache cacheService;

        public WinlossService(IDynamicRepository dynamicRepository, IMemoryCache cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public Task<EnteredSportResult> GetEnteredSportResults()
            => GetEnteredSportResultsAsync();

        public async Task<EnteredSportResult> GetEnteredSportResultsAsync()
        {
            bool includeTestData = AppSettings.LocalTest;
            var reportData = await GetDataEnteredSportResults(includeTestData);

            return GetTotalEnteredSportYesterdayAndToday(reportData);
        }

        internal static EnteredSportResult GetTotalEnteredSportYesterdayAndToday(IEnumerable<EnteredSportResult> reportData)
        {
            int totalEnteredToday = 0;
            int totalNonEnteredToday = 0;
            int totalEnteredYesterday = 0;
            int totalNonEnteredYesterday = 0;

            if (reportData.Any())
            {
                foreach (var data in reportData)
                {
                    if (data.EnteredSportToday != null)
                    {
                        totalEnteredToday += data.EnteredSportToday.Entered;
                        totalNonEnteredToday += data.EnteredSportToday.NonEntered;
                    }

                    if (data.EnteredSportYesterday != null)
                    {
                        totalEnteredYesterday += data.EnteredSportYesterday.Entered;
                        totalNonEnteredYesterday += data.EnteredSportYesterday.NonEntered;
                    }
                }
            }

            var totalToday = new DailyEnteredSport(totalEnteredToday, totalNonEnteredToday);
            var totalYesterday = new DailyEnteredSport(totalEnteredYesterday, totalNonEnteredYesterday);

            return new EnteredSportResult(0, "Total", totalToday, totalYesterday);
        }

        internal async Task<IEnumerable<EnteredSportResult>> GetDataEnteredSportResults(bool includeTestData)
        {
            var dailyEnteredSportData = await cacheService.GetOrCreateAsync(
                EnteredResultsCacheKey,
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
                    var criteria = new DailyEnteredSportCriteria(includeTestData);
                    return dynamicRepository.FetchAsync<DailyEnteredSport>(criteria);
                });

            if (dailyEnteredSportData.Any())
            {
                return dailyEnteredSportData.GroupBy(
                    dailyEnteredSport => dailyEnteredSport.SportType,
                    (sportType, dailyEnteredSports) => new EnteredSportResult(
                        sportType,
                        sportType.ToString(),
                        dailyEnteredSports.FirstOrDefault(enteredSportResult => enteredSportResult.IsToday),
                        dailyEnteredSports.FirstOrDefault(enteredSportResult => !enteredSportResult.IsToday)));
            }

            return [];
        }
    }
}