using AlphaPlus.Settings.Transfer.API.Extensions;
using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers
{
    public class TransferToDownlineHistoryHandler :
        TransferToDownlineHistoryBaseHandler,
        IRequestHandler<TransferToDownlineHistoryQuery, TransferToDownlineHistoryViewModel>,
        IRequestHandler<TransferToDownlineHistoryPagingQuery, TransferToDownlineHistoryPagingViewModel>
    {
        public TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
            : base(transferToDownlineHistoryService)
        {
        }

        public Task<TransferToDownlineHistoryViewModel> Handle(TransferToDownlineHistoryQuery message, CancellationToken cancellation)
        {
            return Task.FromResult(new TransferToDownlineHistoryViewModel(message, new TransferToDownlineHistoryParam(
                message.CustId,
                message.UserName,
                message.FromDate,
                message.ToDate)));
        }

        public async Task<TransferToDownlineHistoryPagingViewModel> Handle(TransferToDownlineHistoryPagingQuery message, CancellationToken cancellation)
        {
            var (transferToDownlines, totalRecords) = await GetTransferToDownlineHistory(new TransferToDownlineHistoryPagingParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.PageSize,
                message.PageIndex));

            return new TransferToDownlineHistoryPagingViewModel(transferToDownlines, totalRecords);
        }
    }
}