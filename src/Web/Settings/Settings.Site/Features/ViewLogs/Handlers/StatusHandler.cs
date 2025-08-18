using System.Web;
using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Params;
using AlphaPlus.Settings.ViewLogs.API.Services;
using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Status;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public class StatusHandler : BaseHandler, IRequestHandler<IndexQuery, LogViewModel>
    {
        private readonly TrackingValueFormatter trackingValueFormatter;
        private readonly TransformService transformService;
        private readonly Fanex.Logging.ILogger logger;

        public StatusHandler(
            [FromKeyedServices("statusLogSvc")] LogService logService,
            LogFormatter logFormatter,
            IDateTime dateTimeWrapper,
            TransformService transformService,
            TrackingValueFormatter trackingValueFormatter,
            Fanex.Logging.ILogger logger,
            IUserSession userSession) :
             base(logService, logFormatter, dateTimeWrapper, userSession)
        {
            this.trackingValueFormatter = trackingValueFormatter;
            transformService.Init(logFormatter);
            this.transformService = transformService;
            this.logger = logger;
        }

        public async Task<LogViewModel> Handle(IndexQuery message, CancellationToken cancellation)
        {
            var logsParam = CreateLogParam(message);
            var pagingLogs = await LogService.GetPagingLogs(logsParam);

            var viewModel = new LogViewModel(logsParam, pagingLogs.TotalRecords)
            {
                ActionTypeId = message.ActionTypeId,
                IsOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(_userSession.Site)
            };

            await TransformLogs(pagingLogs, message, viewModel);

            return viewModel;
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
                    var htmlTrackingValue = transformService.TransformTrackingValue(formattedTrackingValue, log.ActionId, transformParam);

                    if (htmlTrackingValue != null)
                    {
                        logItems.Add(
                            new LogItemViewModel
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