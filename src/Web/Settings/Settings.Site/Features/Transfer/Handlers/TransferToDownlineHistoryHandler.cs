using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferToDownlineHistoryHandler(TransferToDownlineHistoryService transferToDownlineHistoryService) :
    TransferToDownlineHistoryBaseHandler(transferToDownlineHistoryService),
    IRequestHandler<TransferToDownlineHistoryPagingQuery, TransferToDownlineHistoryPagingViewModel>
{
    public async Task<TransferToDownlineHistoryPagingViewModel> Handle(TransferToDownlineHistoryPagingQuery message, CancellationToken cancellation)
    {
        var (transferToDownlines, totalRecords) = await GetTransferToDownlineHistory(message);

        return new TransferToDownlineHistoryPagingViewModel(transferToDownlines, totalRecords);
    }
}