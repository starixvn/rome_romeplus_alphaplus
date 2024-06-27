namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers
{
    using AlphaPlus.Settings.Transfer.API.Extensions;
    using AlphaPlus.Settings.Transfer.API.Params.TransferToDownlineHistory;
    using AlphaPlus.Settings.Transfer.API.Services;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryHandler :
        TransferToDownlineHistoryBaseHandler,
        IRequestHandler<TransferToDownlineHistoryQuery, TransferToDownlineHistoryViewModel>,
        IRequestHandler<TransferToDownlineHistoryPagingQuery, TransferToDownlineHistoryPagingViewModel>
    {
        public TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService)
            : base(transferToDownlineHistoryService)
        {
        }

        public async Task<TransferToDownlineHistoryViewModel> Handle(TransferToDownlineHistoryQuery message, CancellationToken cancellation)
        {
            return new TransferToDownlineHistoryViewModel(message, new TransferToDownlineHistoryParam(
                message.CustId,
                message.UserName,
                message.FromDate,
                message.ToDate));
        }

        public async Task<TransferToDownlineHistoryPagingViewModel> Handle(TransferToDownlineHistoryPagingQuery message, CancellationToken cancellation)
        {
            var data = GetTransferToDownlineHistory(new TransferToDownlineHistoryPagingParam(
                message.CustId,
                message.UserName,
                message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
                message.ToDate,
                message.PageSize,
                message.PageIndex));

            return new TransferToDownlineHistoryPagingViewModel(data.Item1, data.Item2);
        }
    }
}