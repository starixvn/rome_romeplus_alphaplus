using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Services;
using Fanex.Common;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Credit;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;
using AlphaPlus.Settings.ViewLogs.API.Extensions;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public class CreditHandler : BaseHandler, IRequestHandler<IndexQuery, LogViewModel>
    {
        public CreditHandler(
            [FromKeyedServices("creditLogSvc")] LogService logService, 
            LogFormatter logFormatter, 
            IDateTime dateTimeWrapper,
            IUserSession userSession) :
            base(logService, logFormatter, dateTimeWrapper, userSession)
        {
        }

        public async Task<LogViewModel> Handle(IndexQuery message, CancellationToken cancellation)
        {
            var logsParam = CreateLogParam(message);
            var pagingLogs = await LogService.GetPagingLogs(logsParam);
            var viewModel = new LogViewModel(logsParam, pagingLogs.TotalRecords);

            await TransformLogs(pagingLogs, message, viewModel);

            viewModel.ActionTypeId = message.ActionTypeId;
            viewModel.IsOfferedCashService = AlphaPlus.Core.Configurations.AppSettings.IsOfferedCashService(_userSession.Site);
            return viewModel;
        }

        public async Task TransformLogs(
            PagingLogs<Log> pagingLogs,
            LogQuery message,
            LogViewModel viewModel)
        {
            var logItems = new List<LogItemViewModel>();

            foreach (var log in pagingLogs.Logs)
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

                    TrackingValue = Converter.ToDecimal(log.TrackingValue).ToNumericString(),
                    TrackingValueOld = Converter.ToDecimal(log.TrackingValueOld).ToNumericString(),
                    Details = log.Details,
                    IP = LogFormatter.FormatIP(log.IP)
                });
            }

            viewModel.LogItems = logItems;
        }
    }
}