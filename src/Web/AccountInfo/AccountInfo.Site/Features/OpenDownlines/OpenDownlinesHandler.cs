using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests;
using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines
{
    public class OpenDownlinesHandler :
        IRequestHandler<OpenViewListRequest, OpenListViewModel>,
        IRequestHandler<OpenSingleRequest, OpenSingleViewModel>,
        IRequestHandler<OpenMultipleRequest, OpenMultipleViewModel>,
        IRequestHandler<OpenSingleCommand, Result>,
        IRequestHandler<OpenMultipleCommand, OpenMultipleResultViewModel>
    {
        private readonly OpenDownlinesService openDownlinesService;
        private readonly ISearchAccountService searchAccountService;

        public OpenDownlinesHandler(
            OpenDownlinesService openDownlinesService,
            ISearchAccountService searchAccountService)
        {
            this.openDownlinesService = openDownlinesService;
            this.searchAccountService = searchAccountService;
        }

        public async Task<OpenListViewModel> Handle(OpenViewListRequest message, CancellationToken cancellationToken)
            => new OpenListViewModel(
                message.SearchAccountId,
                message.SearchAccountName,
                await openDownlinesService.GetOpenViewListModel(
                    message.SearchAccountId,
                    message.IsAffected,
                    message.DownlineRoleId,
                    message.PageSize,
                    message.PageIndex).ConfigureAwait(false));

        public async Task<OpenSingleViewModel> Handle(OpenSingleRequest message, CancellationToken cancellationToken)
            => new OpenSingleViewModel(
                new OpenSingleDataViewModel(
                    message.SearchAccountId,
                    message.SearchAccountName,
                    await openDownlinesService.GetOpenInfoModel(message.SearchAccountId)
                    .ConfigureAwait(false)));

        public async Task<OpenMultipleViewModel> Handle(OpenMultipleRequest message, CancellationToken cancellationToken)
        => new OpenMultipleViewModel(
                message.RoleId,
                await openDownlinesService.GetOpenMultipleModel(message.CustIds, message.RoleId).ConfigureAwait(false));

        public async Task<Result> Handle(OpenSingleCommand message, CancellationToken cancellationToken)
        => await searchAccountService.UpdateCustomerStatus(
              new UpdateCustomerStatusParam(
                  message.SearchAccountId,
                  message.ViewerId,
                  message.SubAccountId,
                  message.Language,
                  message.IP,
                  false,
                  StatusNames.Closed,
                  message.ViewerName)).ConfigureAwait(false);

        public async Task<OpenMultipleResultViewModel> Handle(OpenMultipleCommand message, CancellationToken cancellationToken)
        {
            var custIds = string.Join("^", message.OpenList.Select(opened => opened.SearchAccountId));
            var isValidCheckMultipleCustomesBelonging = await searchAccountService.CheckMultipleCustomersBelonging(message.ViewerId, message.SubAccountId, custIds);

            if (isValidCheckMultipleCustomesBelonging)
            {
                var tasks = new List<Task<OpenResult>>();
                foreach (var customer in message.OpenList)
                {
                    tasks.Add(UpdateCloseAsync(message, customer));
                }

                return new OpenMultipleResultViewModel(message.RoleId, await Task.WhenAll(tasks).ConfigureAwait(false));
            }

            return new OpenMultipleResultViewModel(
                message.RoleId,
                [
                    new OpenResult("", true, new Result(StatusCodes.Status403Forbidden,"You are not authorized on this function."))
                ]);
        }

        internal async Task<OpenResult> UpdateCloseAsync(OpenMultipleCommand message, OpenCustomerInfo customer)
        => new OpenResult(
           customer.Username,
           customer.IsClosed,
           await searchAccountService.UpdateCustomerStatus(
                  new UpdateCustomerStatusParam(
                      customer.SearchAccountId,
                      message.ViewerId,
                      message.SubAccountId,
                      message.Language,
                      message.IP,
                      customer.IsClosed,
                      StatusNames.Closed,
                      message.ViewerName)).ConfigureAwait(false));
    }
}