using AlphaPlus.Core.Models;
using AlphaPlus.Settings.ViewLogs.API.Criteria;
using AlphaPlus.Settings.ViewLogs.API.Criteria.Status;
using AlphaPlus.Settings.ViewLogs.API.DataAccesses;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Params;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl.Logs
{
    public class StatusServiceImpl : BaseService, LogService
    {
        private const int ActionTypeId = 5;

        public StatusServiceImpl(
            IDynamicRepository dynamicRepository,
            CustomerService customerService,
            LogDataAccess baseDataAccess,
            IUserSession session,
            IMemoryCache cacheService) :
            base(dynamicRepository, customerService, baseDataAccess, session, cacheService)
        {
        }

        internal override ICriteria CreateGetLogsCriteria(LogParam logParam)
        {
            if (logParam == null)
            {
                throw new ArgumentNullException(nameof(logParam));
            }

            var fromDate = GetGreaterDate(logParam.FromDate, logParam.LimitedDateToShowInfo);

            return new GetStatusLogsCriteria(
                logParam.CustId,
                logParam.Username ?? string.Empty,
                ActionTypeId,
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
                ActionTypeId,
                DateTimeHelper.GetBeginningOfDate(fromDate),
                DateTimeHelper.GetEndingOfDate(logParam.ToDate),
                logParam.IsInternal);
        }
    }
}
