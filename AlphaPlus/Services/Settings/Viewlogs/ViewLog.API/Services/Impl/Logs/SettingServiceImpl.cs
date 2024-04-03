namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.ViewLogs.API.Models;
    using AlphaPlus.Core.Models;
    using Criteria;
    using Criteria.Setting;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Helpers;
    using Params;

    public class SettingServiceImpl : BaseService, LogService
    {
        private const int RNGSlotBetLimitActionId = 391;

        public SettingServiceImpl(
             IDynamicRepository dynamicRepository,
             CustomerService customerService,
             LogDataAccess baseDataAccess,
             IUserSession session,
             ICacheService cacheService) :
            base(dynamicRepository, customerService, baseDataAccess, session, cacheService)
        {
        }

        protected override IEnumerable<Log> GetLogs(LogParam logParam)
        {
            var logs = base.GetLogs(logParam);

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
                logParam.Username,
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
            (log.TrackingValue.Contains("ProductID=\"18\"") || log.TrackingValue.Contains("ProductID=\"22\""));
    }
}
