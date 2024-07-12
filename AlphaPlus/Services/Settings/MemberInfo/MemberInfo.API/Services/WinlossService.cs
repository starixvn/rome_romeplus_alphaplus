using AlphaPlus.Settings.MemberInfo.API.Criteria.WinLoss;
using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
using Fanex.Caching;
using Fanex.Data.Repository;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace AlphaPlus.Settings.MemberInfo.API.Services
{
    public interface IWinlossService
    {
        EnteredSportResult GetEnteredSportResults();

        Task<EnteredSportResult> GetEnteredSportResultsAsync();
    }

    public class WinlossService : IWinlossService
    {
        private const string EnteredResultsCacheKey = "Alpha.Reports.Services.Impl.WinLossServiceImpl.EnteredResults";
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public WinlossService(IDynamicRepository dynamicRepository, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public EnteredSportResult GetEnteredSportResults()
            => GetEnteredSportResultsAsync().GetAwaiter().GetResult();

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
            var dailyEnteredSportData = await cacheService.GetOrSetAsync(
                EnteredResultsCacheKey,
                () =>
                {
                    var criteria = new DailyEnteredSportCriteria(includeTestData);
                    return dynamicRepository.FetchAsync<DailyEnteredSport>(criteria);
                },
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));

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