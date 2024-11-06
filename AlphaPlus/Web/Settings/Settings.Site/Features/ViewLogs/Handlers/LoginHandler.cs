using AlphaPlus.Settings.ViewLogs.API.Helpers;
using AlphaPlus.Settings.ViewLogs.API.Models;
using AlphaPlus.Settings.ViewLogs.API.Services;
using MediatR;
using AlphaPlus.Settings.Site.Features.ViewLogs.Requests.Login;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels;
using AlphaPlus.Settings.Site.Features.ViewLogs.ViewModels.Login;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Handlers
{
    public class LoginHandler : BaseHandler, IRequestHandler<IndexQuery, IndexViewModel>
    {
        public LoginHandler([FromKeyedServices("loginLogSvc")] LogService logService, LogFormatter logFormatter, IDateTime dateTimeWrapper) :
            base(logService, logFormatter, dateTimeWrapper)
        {
        }

        public async Task<IndexViewModel> Handle(IndexQuery message, CancellationToken cancellation)
        {
            var logsParam = CreateLogParam(message);
            var pagingLogs = await LogService.GetPagingLogs(logsParam);

            var viewModel = new IndexViewModel(logsParam, pagingLogs.TotalRecords);

            await TransformLogs(pagingLogs, message, viewModel);

            viewModel.PreviousUsername = message.PreviousUsername;
            viewModel.ActionTypeId = message.ActionTypeId;
            viewModel.IsAgent = message.IsAgentViewing;
            viewModel.Username = message.IsAgentViewing && message.PreviousUsername.Equals(message.Username)
                ? string.Empty
                : message.Username;

            return viewModel;
        }

        private async Task TransformLogs(
            PagingLogs<Log> pagingLogs,
            IndexQuery message,
            LogViewModel viewModel)
        {
            var logItems = new List<LogItemViewModel>();

            foreach (var log in pagingLogs.Logs)
            {
                logItems.Add(new LoginLogItemViewModel
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

                    Details = log.Details,
                    IP = LogFormatter.FormatIP(log.IP),

                    Domain = log.Domain,
                    Browser = log.Browser,
                    Country = LogFormatter.FormatCountryCode(log.IP, log.Country)
                });
            }

            viewModel.LogItems = logItems;
        }
    }
}