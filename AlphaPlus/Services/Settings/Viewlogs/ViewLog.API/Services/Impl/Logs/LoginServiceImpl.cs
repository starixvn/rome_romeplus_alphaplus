namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Core.Models;
    using Constants;
    using Criteria.Login;
    using DataAccesses;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Helpers;
    using Models;
    using Params;

    public class LoginServiceImpl : BaseService, LogService
    {
        public LoginServiceImpl(
            IDynamicRepository dynamicRepository,
            CustomerService customerService,
            LogDataAccess logDataAccess,
            IUserSession session,
            ICacheService cacheService) :
            base(dynamicRepository, customerService, logDataAccess, session, cacheService)
        {
        }

        protected override IEnumerable<Log> GetLogs(LogParam logParam)
        {
            var criteria = CreateGetLogsCriteria(logParam);

            return LogDataAccess.GetLogs(criteria);
        }

        internal override ICriteria CreateGetLogsCriteria(LogParam logParam)
        {
            if (logParam == null)
            {
                throw new ArgumentNullException(nameof(logParam));
            }

            var fromDate = GetGreaterDate(logParam.FromDate, logParam.LimitedDateToShowInfo);

            return new GetLoginLogsCriteria(
                logParam.CustId,
                logParam.Username ?? string.Empty,
                DateTimeHelper.GetBeginningOfDate(fromDate),
                DateTimeHelper.GetEndingOfDate(logParam.ToDate),                
                logParam.IsInternal ? LoginLogType.InternalAlpha : LoginLogType.Alpha,
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

            return new GetTotalLoginLogsCriteria(
                logParam.CustId,
                logParam.Username ?? string.Empty,
                DateTimeHelper.GetBeginningOfDate(fromDate),
                DateTimeHelper.GetEndingOfDate(logParam.ToDate),                
                logParam.IsInternal ? LoginLogType.InternalAlpha : LoginLogType.Alpha);
        }
    }
}
