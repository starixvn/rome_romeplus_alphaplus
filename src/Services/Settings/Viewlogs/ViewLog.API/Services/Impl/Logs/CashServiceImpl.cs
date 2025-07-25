using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Criteria;
using AlphaPlus.Settings.ViewLogs.API.Criteria.CashService;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    public class CashServiceImpl : BaseService, LogService
    {
        private const int RNGSlotBetLimitActionId = 391;
        private const string CashServiceLogActionTypesCacheKey = "AlphaPlus.Settings.ViewLogs.API.CashService.LogActionTypes";
        
        public CashServiceImpl(
             IDynamicRepository dynamicRepository,
             CustomerService customerService,
             LogDataAccess baseDataAccess,
             IUserSession session,
             IMemoryCache cacheService) :
            base(dynamicRepository, customerService, baseDataAccess, session, cacheService)
        {
        }

        public override async Task<IEnumerable<LogActionType>> GetLogActionTypes()
        {
            var logActionTypes = cacheService.Get<IEnumerable<LogActionType>>(CashServiceLogActionTypesCacheKey);

            if (logActionTypes != null
                && logActionTypes.Any())
            {
                return logActionTypes;
            }

            logActionTypes = await dynamicRepository.FetchAsync<LogActionType>(new GetCashServiceLogActionTypesCriteria());

            cacheService.Set(CashServiceLogActionTypesCacheKey, logActionTypes, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromHours(OneHour) });

            return logActionTypes;
        }

        protected override async Task<IEnumerable<Log>> GetLogs(LogParam logParam)
        {
            var logs = await base.GetLogs(logParam);

            return logs.Where(i => !IsBetLimitOfRNGSlot(i));
        }

        internal override ICriteria CreateGetLogsCriteria(LogParam logParam)
        {
            if (logParam == null)
            {
                throw new ArgumentNullException(nameof(logParam));
            }

            var fromDate = GetGreaterDate(logParam.FromDate, logParam.LimitedDateToShowInfo);

            return new GetCashServiceLogsCriteria(
                logParam.CustId,
                logParam.Username ?? string.Empty,
                logParam.ActorId,
                ((CashServiceParam)logParam).ActionTypeId,
                DateTimeHelper.GetBeginningOfDate(fromDate),
                DateTimeHelper.GetEndingOfDate(logParam.ToDate),
                logParam.IsInternal,
                logParam.PageIndex,
                logParam.PageSize);
        }

        internal override ICriteria CreateGetTotalLogsCriteria(LogParam logParam)
        {
            if (logParam == null)
            {
                throw new ArgumentNullException(nameof(logParam));
            }

            var fromDate = GetGreaterDate(logParam.FromDate, logParam.LimitedDateToShowInfo);

            return new GetTotalCashServiceLogsCriteria(
                logParam.CustId,
                logParam.ActorId,
                ((CashServiceParam)logParam).ActionTypeId,
                DateTimeHelper.GetBeginningOfDate(fromDate),
                DateTimeHelper.GetEndingOfDate(logParam.ToDate),
                logParam.IsInternal);
        }

        static private bool IsBetLimitOfRNGSlot(Log log)
            => log.ActionId == RNGSlotBetLimitActionId &&
            (log.TrackingValue != null && log.TrackingValue.Contains("ProductID=\"18\"") || log.TrackingValue != null && log.TrackingValue.Contains("ProductID=\"22\""));
        
    }
}
