using System.Web;
using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.CashService;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels.CashService;
using AlphaPlus.Settings.ViewLogs.API.Constants;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;
using AlphaPlus.Settings.ViewLogs.API.Services;
using AlphaPlus.Shared.AppResources.Settings.ViewLog;
using Fanex.Common;
using MediatR;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public class CashServiceHandler : BaseHandler, IRequestHandler<IndexQuery, IndexViewModel>
    {
        private readonly TrackingValueFormatter trackingValueFormatter;
        private readonly TransformService transformService;
        private readonly Fanex.Logging.ILogger logger;

        public CashServiceHandler(
            [FromKeyedServices("cashServiceLogSvc")] LogService logService,
            LogFormatter logFormatter,
            IDateTime dateTimeWrapper,
            TransformService transformService,
            TrackingValueFormatter trackingValueFormatter,
            Fanex.Logging.ILogger logger,
            IUserSession userSession) :
             base(logService, logFormatter, dateTimeWrapper, userSession)
        {
            this.trackingValueFormatter = trackingValueFormatter;
            this.transformService = transformService;
            this.logger = logger;
        }

        public async Task<IndexViewModel> Handle(IndexQuery message, CancellationToken cancellation)
        {
            var fromDate = GetFromDate(message);
            var toDate = GetToDate(message);

            var pageSize = string.IsNullOrWhiteSpace(message.PageSize) ? DefaultVariables.PageSize : Converter.ToInt(message.PageSize);

            var logsParam = new CashServiceParam
            {
                ActorId = message.ActorId,
                UplineId = message.UplineId,
                SubAccId = message.SubAccId,
                RoleId = message.RoleId,
                FromDate = fromDate,
                ToDate = toDate,
                Username = message.Username,
                IsInternal = message.IsInternal,

                PageSize = pageSize == 0 ? DefaultVariables.PageSize : pageSize,
                PageIndex = string.IsNullOrWhiteSpace(message.PageIndex) ? 0 : Converter.ToInt(message.PageIndex),

                Language = message.Language,
                HasViewLogPermission = message.HasViewLogPermission,

                MaxReportDate = message.MaxReportDate,
                MinReportDate = message.MinReportDate,
                IsShowOldValue = message.IsShowOldValue,
                ActionTypeId = string.IsNullOrWhiteSpace(message.ActionTypeId) ? ActionType.All : Converter.ToInt(message.ActionTypeId),
                LimitedDateToShowInfo = message.LimitedDateForShowInfo
            };

            var pagingLogs = await LogService.GetPagingLogs(logsParam);
            var viewModel = new IndexViewModel(logsParam, pagingLogs.TotalRecords)
            {
                LogActionTypes = await BuildLogActionTypeViewModels(message),
                ActionTypeId = message.ActionTypeId,
                IsOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(_userSession.Site)
            };

            await TransformLogs(pagingLogs, message, viewModel);

            return viewModel;
        }
        
        private async Task<IEnumerable<LogActionTypeViewModel>> BuildLogActionTypeViewModels(IndexQuery message)
        {
            var logActionTypes = await LogService.GetLogActionTypes();

            var logActionTypeViewModels = logActionTypes
                .Select(logActionType => new LogActionTypeViewModel(logActionType, message.Language))
                .ToList();

            logActionTypeViewModels.Insert(0, new LogActionTypeViewModel(ActionType.All, Resources.AllTypes));

            return logActionTypeViewModels;
        }

        private async Task TransformLogs(
            PagingLogs<Log> pagingLogs,
            IndexQuery message,
            LogViewModel viewModel)
        {
            var transformParam = new TransformParam
            {
                IsShowOldValue = message.IsShowOldValue,
                TrackingValueTransformTemplatePath = message.TrackingValueTransformTemplatePath,
                CustStatuses = message.SportBetTypeVisibilities,
                ResourcesByBetTypeCacheRef = message.ResourcesByBetTypeCacheRef,
                ResourcesProductCacheRef = message.ResourcesByProductCacheRef,
                ResourcesBySportTypeCacheRef = message.ResourcesBySportTypeCacheRef
            };

            var logItems = new List<LogItemViewModel>();

            foreach (var log in pagingLogs.Logs)
            {
                try
                {
                    var formattedTrackingValue = trackingValueFormatter.GetFormattedDiffTrackingValue(log, message.IsShowOldValue);
                    var htmlTrackingValue = transformService.TransformTrackingValue(formattedTrackingValue, log.ActionId, transformParam, log.IsHideNickname);

                    if (htmlTrackingValue != null)
                    {
                        logItems.Add(new LogItemViewModel
                        {
                            ActionId = log.ActionId,
                            ActionName = await LogService.GetLogActionName(log.ActionId, message.Language),

                            ActionTypeId = log.ActionTypeId,
                            CustId = log.CustId,
                            CustName = log.CustName,
                            LogDate = DateTimeHelper.ToLongDateWithSlash(log.LogDate),
                            LogId = log.LogId,
                            SPName = log.StoreProcedureName,
                            UserId = log.UserId,
                            Username = log.Username,
                            TrackingValue = HttpUtility.HtmlDecode(htmlTrackingValue),
                            IP = LogFormatter.FormatIP(log.IP)
                        });
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message, ex);
                }
            }

            viewModel.LogItems = logItems;
        }
    }
}