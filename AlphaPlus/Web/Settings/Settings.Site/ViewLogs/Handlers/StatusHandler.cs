namespace AlphaPlus.Settings.Site.ViewLogs.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using AlphaPlus.Settings.ViewLogs.API.Helpers;
    using AlphaPlus.Settings.ViewLogs.API.Models;
    using AlphaPlus.Settings.ViewLogs.API.Params;
    using AlphaPlus.Settings.ViewLogs.API.Services;
    using Fanex.Logging.Extensions.Web;
    using Helpers;
    using MediatR;
    using Requests.Status;
    using ViewModels;

    public class StatusHandler : BaseHandler, IRequestHandler<IndexQuery, LogViewModel>
    {
        private readonly TrackingValueFormatter trackingValueFormatter;
        private readonly TransformService transformService;

        public StatusHandler(
            [FromKeyedServices("statusLogSvc")]LogService logService,
            LogFormatter logFormatter,
            IDateTime dateTimeWrapper,
            TransformService transformService,
            TrackingValueFormatter trackingValueFormatter) :
             base(logService, logFormatter, dateTimeWrapper)
        {
            this.trackingValueFormatter = trackingValueFormatter;
            this.transformService = transformService;
        }

        public async Task<LogViewModel> Handle(IndexQuery message, CancellationToken cancellation)
        {
            var logsParam = CreateLogParam(message);
            var pagingLogs = LogService.GetPagingLogs(logsParam);

            var viewModel = new LogViewModel(logsParam, pagingLogs.TotalRecords);
            viewModel.ActionTypeId = message.ActionTypeId;

            TransformLogs(pagingLogs, message, viewModel);

            return viewModel;
        }

        private void TransformLogs(
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
                IsShowMyanmarOdds = message.IsShowMyanmarOdds,
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
                                ActionName = LogService.GetLogActionName(log.ActionId, message.Language),

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
                    ExceptionHandler.Handle(ex);
                }
            }

            viewModel.LogItems = logItems;
        }
    }
}