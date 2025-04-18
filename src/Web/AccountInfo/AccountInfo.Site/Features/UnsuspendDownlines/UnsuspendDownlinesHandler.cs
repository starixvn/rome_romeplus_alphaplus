using AlphaPlus.AccountInfo.Site.Features.SearchAccount;
using AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.CustomerStatus;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines
{
    public class UnsuspendDownlinesHandler :
        IRequestHandler<UnsuspendViewListRequest, UnsuspendListViewModel>,
        IRequestHandler<UnsuspendSingleRequest, UnsuspendSingleViewModel>,
        IRequestHandler<UnsuspendMultipleRequest, UnsuspendMultipleViewModel>,
        IRequestHandler<UnsuspendSingleCommand, Result>,
        IRequestHandler<UnsuspendMultipleCommand, UnsuspendMultipleResultViewModel>,
        IRequestHandler<SuspendMultipleConfirmRequest, SuspendMultipleConfirmViewModel>
    {
        private readonly IUnsuspendDownlinesService unsuspendDownlinesService;
        private readonly ISearchAccountService accountInfoService;

        public UnsuspendDownlinesHandler(
            IUnsuspendDownlinesService unsuspendDownlinesService,
            ISearchAccountService accountInfoService)
        {
            this.unsuspendDownlinesService = unsuspendDownlinesService;
            this.accountInfoService = accountInfoService;
        }

        public async Task<UnsuspendSingleViewModel> Handle(UnsuspendSingleRequest message, CancellationToken cancellationToken)
            => new UnsuspendSingleViewModel(
                new UnsuspendSingleDataViewModel(
                message.SearchAccountId,
                message.SearchAccountName,
                await unsuspendDownlinesService.GetUnsuspendInfoModel(message.SearchAccountId)));

        public async Task<UnsuspendListViewModel> Handle(UnsuspendViewListRequest message, CancellationToken cancellationToken)
             => new UnsuspendListViewModel(
                 message.SearchAccountId,
                 message.SearchAccountName,
                 await unsuspendDownlinesService.GetUnsuspendViewListModel(
                     message.SearchAccountId,
                     message.IsAffected,
                     message.DownlineRoleId,
                     message.PageSize,
                     message.PageIndex));

        public async Task<UnsuspendMultipleViewModel> Handle(UnsuspendMultipleRequest message, CancellationToken cancellationToken)
            => new UnsuspendMultipleViewModel(
                message.RoleId,
                await unsuspendDownlinesService.GetUnsuspendMultipleModel(message.CustIds, message.RoleId));

        public async Task<Result> Handle(UnsuspendSingleCommand message, CancellationToken cancellationToken)
          => await accountInfoService.UpdateCustomerStatus(
              new UpdateCustomerStatusParam(
                  message.SearchAccountId,
                  message.ViewerId,
                  message.SubAccountId,
                  message.Language,
                  message.IP,
                  false,
                  StatusNames.Suspended,
                  message.ViewerName));

        public async Task<UnsuspendMultipleResultViewModel> Handle(UnsuspendMultipleCommand message, CancellationToken cancellationToken)
        {
            var custIds = string.Join("^", message.UnsuspendList.Select(unspended => unspended.SearchAccountId));
            var isValidCheckMultipleCustomesBelonging = await accountInfoService.CheckMultipleCustomersBelonging(message.ViewerId, message.SubAccountId, custIds);

            if (isValidCheckMultipleCustomesBelonging)
            {
                var tasks = new List<Task<UnsuspendResult>>();
                foreach (var customer in message.UnsuspendList)
                {
                    tasks.Add(UpdateSuspendAsync(message, customer));
                }

                return new UnsuspendMultipleResultViewModel(message.RoleId, await Task.WhenAll(tasks).ConfigureAwait(false));
            }

            return new UnsuspendMultipleResultViewModel(message.RoleId,
                [
                    new UnsuspendResult("", true, new Result(StatusCodes.Status403Forbidden,"You are not authorized on this function."))
                ]);
        }

        public Task<SuspendMultipleConfirmViewModel> Handle(SuspendMultipleConfirmRequest message, CancellationToken cancellationToken)
            => Task.FromResult(new SuspendMultipleConfirmViewModel(message));

        internal async Task<UnsuspendResult> UpdateSuspendAsync(UnsuspendMultipleCommand message, UnsuspendCustomerInfo customer)
            => new UnsuspendResult(
                customer.Username,
                customer.Suspended,
                await accountInfoService.UpdateCustomerStatus(
                    new UpdateCustomerStatusParam(
                    customer.SearchAccountId,
                    message.ViewerId,
                    message.SubAccountId,
                    message.Language,
                    message.IP,
                    customer.Suspended,
                    StatusNames.Suspended,
                    message.ViewerName)));
    }
}