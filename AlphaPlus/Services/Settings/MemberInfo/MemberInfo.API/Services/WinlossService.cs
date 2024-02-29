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
        {
            bool includeTestData = AppSettings.LocalTest;

            var reportData = GetDataEnteredSportResults(includeTestData);

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

        internal IEnumerable<EnteredSportResult> GetDataEnteredSportResults(bool includeTestData)
        {
            var criteria = new DailyEnteredSportCriteria(includeTestData);

            var dailyEnteredSportData = cacheService.GetOrSet(EnteredResultsCacheKey,
                () => dynamicRepository.Fetch<DailyEnteredSport>(criteria),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));

            if (dailyEnteredSportData.Any())
            {
                return dailyEnteredSportData.GroupBy(dailyEnteredSport => dailyEnteredSport.SportType)
                    .Select(dailyEnteredSports => new EnteredSportResult(
                    dailyEnteredSports.Key,
                    dailyEnteredSports.Key.ToString(),
                    dailyEnteredSports.FirstOrDefault(enteredSportResult => enteredSportResult.IsToday),
                    dailyEnteredSports.FirstOrDefault(enteredSportResult => !enteredSportResult.IsToday)));
            }

            return Enumerable.Empty<EnteredSportResult>();
        }
    }
}