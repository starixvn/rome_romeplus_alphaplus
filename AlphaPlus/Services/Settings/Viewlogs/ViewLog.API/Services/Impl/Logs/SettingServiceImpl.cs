using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Criteria;
using AlphaPlus.Settings.ViewLogs.API.Criteria.Setting;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    public class SettingServiceImpl : BaseService, LogService
    {
        private const int RNGSlotBetLimitActionId = 391;

        public SettingServiceImpl(
             IDynamicRepository dynamicRepository,
             CustomerService customerService,
             LogDataAccess baseDataAccess,
             IUserSession session,
             IMemoryCache cacheService) :
            base(dynamicRepository, customerService, baseDataAccess, session, cacheService)
        {
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

            return new GetSettingLogsCriteria(
                logParam.CustId,
                logParam.Username ?? string.Empty,
                logParam.ActorId,
                ((SettingParam)logParam).ActionTypeId,
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

            return new GetTotalSettingOrStatusLogsCriteria(
                logParam.CustId,
                logParam.ActorId,
                ((SettingParam)logParam).ActionTypeId,
                DateTimeHelper.GetBeginningOfDate(fromDate),
                DateTimeHelper.GetEndingOfDate(logParam.ToDate),
                logParam.IsInternal);
        }

        static private bool IsBetLimitOfRNGSlot(Log log)
            => log.ActionId == RNGSlotBetLimitActionId &&
            (log.TrackingValue != null && log.TrackingValue.Contains("ProductID=\"18\"") || log.TrackingValue != null && log.TrackingValue.Contains("ProductID=\"22\""));
    }
}
