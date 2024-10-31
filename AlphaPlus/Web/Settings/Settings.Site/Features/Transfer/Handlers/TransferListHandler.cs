using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Helpers;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferList;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferList;
using AlphaPlus.Settings.Transfer.API.Params.TransferList;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferListHandler(
    ITransferListService transferListService,
    ITransferService transferService,
    IWinlossService winlossService) : IRequestHandler<TransferListQuery, TransferListViewModel>
{
    private readonly ITransferListService transferListService = transferListService;
    private readonly ITransferService transferService = transferService;
    private readonly IWinlossService winlossService = winlossService;

    public async Task<TransferListViewModel> Handle(TransferListQuery message, CancellationToken cancellation)
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransfer(message.ViewerId);
        var isEnableAutoTransfer = !isAllDayBalanceTransfer && AppSettings.CheckOfferAutoTransferCurrencies(message.Currency);

        var prevADBT = message.ADBT ?? false;
        var isPostBack = message.IsPostBack ?? false;
        message.AllYesterdayBalance = !isPostBack && isAllDayBalanceTransfer ? true : message.AllYesterdayBalance;

        var param = new TransferListParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.Username,
            message.Status,
            message.AutoTransfer,
            message.AllYesterdayBalance,
            message.AllowTotalBalanceTransfer,
            message.PageIndex,
            message.PageSize,
            message.ActorLevel.IsAgentLevel() ? message.AGIP : string.Empty);

        var enteredSportResultTask = winlossService.GetEnteredSportResultsAsync();
        var reportDataTask = transferListService.GetTransferList(param);
        var transferOptionTask = transferService.GetTransferOption(message.ViewerId);
        await Task.WhenAll(enteredSportResultTask, reportDataTask, transferOptionTask);
        var transferOption = await transferOptionTask;

        return new TransferListViewModel(
            message,
            await enteredSportResultTask,
            await reportDataTask,
            isAllDayBalanceTransfer,
            isEnableAutoTransfer,
            transferOption,
            string.Join(',', TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, message.ViewerLevelId)));
    }
}
