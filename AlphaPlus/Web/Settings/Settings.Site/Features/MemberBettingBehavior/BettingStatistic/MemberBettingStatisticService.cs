using AlphaPlus.PartialUM.Services;
using AlphaPlus.Settings.MemberInfo.API.Services;
using Fanex.Caching;
using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.BettingStatistic
{
    public interface IMemberBettingStatisticService
    {
        Task<MemberBettingStatistic> GetMemberBettingStatistics(int custId, DateTime startDate, DateTime endDate, string siteName, string language);

        Task<BettingStatisticExportData> GetMemberBettingStatisticExcelData(int custId, DateTime startDate, DateTime endDate, string siteName, string language);
    }

    public class MemberBettingStatisticService : IMemberBettingStatisticService
    {
        private const string StatisticCacheKey = "Alpha.MemberInfo.Features.MemberBettingBehavior.Statistic";
        private const string StatisticExportCacheKey = "Alpha.MemberInfo.Features.MemberBettingBehavior.StatisticExport";
        private const byte MinuteTimeCache = 5;
        private const string FormatDateForCache = "yyyyMMdd";
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;
        private readonly CacheRefService refService;
        private readonly IPartialUMService partialUMService;

        public MemberBettingStatisticService(
            IDynamicRepository dynamicRepository,
            ICacheService cacheService,
            IPartialUMService partialUMService,
            CacheRefService refService,
            HttpClient httpClient)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
            this.partialUMService = partialUMService;
            this.refService = refService;
        }

        public async Task<MemberBettingStatistic> GetMemberBettingStatistics(int custId, DateTime startDate, DateTime endDate, string siteName, string language)
        {
            var criteria = new GetMemberBettingStatisticsCriteria(custId, startDate, endDate);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

            SummaryStatistic summaryStatistic;
            var turnOverBySports = new List<TurnOverBySport>();
            var turnOverByBettypes = new List<TurnoverByBettype>();
            var turnOverWinLossByDates = new List<TurnoverWinLossByDate>();

            var cacheKey = $"{StatisticCacheKey}.{language}.{custId}.{startDate.ToString(FormatDateForCache)}.{endDate.ToString(FormatDateForCache)}";
            var memberStatisticCacheData = await cacheService.GetAsync<MemberBettingStatistic>(cacheKey);

            if (memberStatisticCacheData != null)
            {
                summaryStatistic = memberStatisticCacheData.SummaryStatistic;
                turnOverBySports.AddRange(memberStatisticCacheData.TurnOverBySports);
                turnOverByBettypes.AddRange(memberStatisticCacheData.TurnoverByBettypes);
                turnOverWinLossByDates.AddRange(memberStatisticCacheData.TurnoverWinLossByDates);

                return new MemberBettingStatistic(summaryStatistic, turnOverBySports, turnOverByBettypes, turnOverWinLossByDates);
            }
            else
            {
                var rawData = await dynamicRepository.FetchMultipleAsync<SummaryStatistic, TurnOverBySport, TurnoverByBettype, TurnoverWinLossByDate>(criteria);
                summaryStatistic = rawData.Item1.FirstOrDefault();
                turnOverBySports.AddRange(rawData.Item2);
                turnOverByBettypes.AddRange(rawData.Item3);
                turnOverWinLossByDates.AddRange(rawData.Item4);
            }

            foreach (var item in turnOverBySports)
            {
                item.SportType = GetMemberStatisticSportTypeName(item.SportType);
            }

            foreach (var item in turnOverByBettypes)
            {
                int.TryParse(item.ProductID, out int productId);
                item.Bettype = GetMemberStatisticBetTypeName(item.Bettype);
                item.ProductName = refService.GetProductName(productId);
            }

            await cacheService.SetAsync(
                cacheKey,
                new MemberBettingStatistic(summaryStatistic, turnOverBySports, turnOverByBettypes, turnOverWinLossByDates),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteTimeCache)));

            return new MemberBettingStatistic(summaryStatistic, turnOverBySports, turnOverByBettypes, turnOverWinLossByDates);
        }

        public async Task<BettingStatisticExportData> GetMemberBettingStatisticExcelData(int custId, DateTime startDate, DateTime endDate, string siteName, string language)
        {
            var criteria = new GetMemberBettingStatisticsExportCriteria(custId, startDate, endDate);
            await partialUMService.ThrowIfUM(criteria.GetSettingKey(), siteName);

            var bettingStatisticExports = new List<BettingStatisticExportRow>();
            BettingStatisticExportRow total;

            var cacheKey = $"{StatisticExportCacheKey}.{language}.{custId}.{startDate.ToString(FormatDateForCache)}.{endDate.ToString(FormatDateForCache)}";
            var memberStatisticExportCacheData = await cacheService.GetAsync<BettingStatisticExportData>(cacheKey);

            if (memberStatisticExportCacheData != null)
            {
                bettingStatisticExports.AddRange(memberStatisticExportCacheData.BettingStatisticExportRows);
                total = memberStatisticExportCacheData.Total;
                return new BettingStatisticExportData(bettingStatisticExports, total);
            }
            else
            {
                var rawData = await dynamicRepository.FetchMultipleAsync<BettingStatisticExportRow, BettingStatisticExportRow>(criteria);
                bettingStatisticExports.AddRange(rawData.Item1);
                total = rawData.Item2.FirstOrDefault();
            }

            foreach (var item in bettingStatisticExports)
            {
                item.Sporttype = GetMemberStatisticSportTypeName(item.Sporttype);
                item.Bettype = GetMemberStatisticBetTypeName(item.Bettype);
            }

            await cacheService.SetAsync(
                  cacheKey,
                  new BettingStatisticExportData(bettingStatisticExports, total),
                  new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(MinuteTimeCache)));

            return new BettingStatisticExportData(bettingStatisticExports, total);
        }

        private string GetMemberStatisticSportTypeName(string sportType)
        {
            int.TryParse(sportType, out int sportTypeId);
            var sportTypeName = refService.GetSportTypeName(sportTypeId);
            return sportTypeName != "" ? sportTypeName : refService.GetCustBetBehavior(sportType);
        }

        private string GetMemberStatisticBetTypeName(string betType)
        {
            int.TryParse(betType, out int betTypeId);
            var betTypeName = refService.GetBetTypeName(betTypeId);
            return betTypeName != "" ? betTypeName : refService.GetCustBetBehavior(betType);
        }
    }
}