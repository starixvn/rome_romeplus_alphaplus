using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.MultipleTransfer;
using AlphaPlus.Settings.Transfer.API.Params.MultipleTransfer;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class MultipleTransferHandler(
    IMultipleTransferService multipleTransferService,
    TransferService transferService,
    IWinlossService winlossService) : IRequestHandler<TransferListQuery, TransferListViewModel>
{
    private readonly IMultipleTransferService multipleTransferService = multipleTransferService;
    private readonly TransferService transferService = transferService;

    public async Task<TransferListViewModel> Handle(TransferListQuery message, CancellationToken cancellation)
    {
        var param = new GetTransferListParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.Username,
            message.Status,
            message.AutoTransfer,
            message.AllYesterdayBalance,
            message.AllowTotalBalanceTransfer,
            message.PageIndex,
            message.PageSize);

        var enteredSportResultTask = winlossService.GetEnteredSportResultsAsync();
        var reportDataTask = multipleTransferService.GetTransferList(param);
        var isAllDayBalanceTransferTask = transferService.IsAllDayBalanceTransferAsync(message.ViewerId);
        await Task.WhenAll(enteredSportResultTask, isAllDayBalanceTransferTask, reportDataTask);

        return new TransferListViewModel(message, await enteredSportResultTask, await reportDataTask, await isAllDayBalanceTransferTask);
    }
}
