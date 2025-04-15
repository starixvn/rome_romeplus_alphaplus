using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Constants;
using AlphaPlus.Settings.ViewLogs.API.Criteria.Login;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    public class LoginServiceImpl : BaseService, LogService
    {
        public LoginServiceImpl(
            IDynamicRepository dynamicRepository,
            CustomerService customerService,
            LogDataAccess logDataAccess,
            IUserSession session,
            IMemoryCache cacheService) :
            base(dynamicRepository, customerService, logDataAccess, session, cacheService)
        {
        }

        protected override Task<IEnumerable<Log>> GetLogs(LogParam logParam)
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
