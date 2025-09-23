using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferList;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferList;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Helpers;
using AlphaPlus.Settings.Transfer.API.Params.TransferList;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferListHandler(
    ITransferListService transferListService,
    ITransferService transferService,
    IWinlossService winlossService) : IRequestHandler<TransferListQuery, TransferListViewModel>
{
    public async Task<TransferListViewModel> Handle(TransferListQuery message, CancellationToken cancellation)
    {
        var currentADBT = await transferService.IsAllDayBalanceTransfer(message.ViewerId);
        var isEnableAutoTransfer = !currentADBT && AppSettings.CheckOfferAutoTransferCurrencies(message.Currency);

        var prevADBT = message.ADBT ?? false;
        var isPostBack = message.IsPostBack ?? false;
        message.AllYesterdayBalance = !isPostBack && currentADBT || message.AllYesterdayBalance;

        var param = new TransferListParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.Username,
            message.Status,
            message.AutoTransfer,
            message.LevelType,
            message.AllYesterdayBalance,
            message.AllowTotalBalanceTransfer,
            message.PageIndex,
            message.PageSize,
            message.SiteDbGroupId);

        var enteredSportResultTask = winlossService.GetEnteredSportResultsAsync();
        var reportDataTask = transferListService.GetTransferList(param);
        var transferOptionTask = transferService.GetTransferOption(message.ViewerId);
        await Task.WhenAll(enteredSportResultTask, reportDataTask, transferOptionTask);
        var transferOption = await transferOptionTask;

        var viewModel = new TransferListViewModel(
            message,
            await enteredSportResultTask,
            await reportDataTask,
            currentADBT,
            isEnableAutoTransfer,
            transferOption,
            string.Join(',', TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(transferOption, message.ViewerLevelId)))
        {
            IsReloadTopPage = (!prevADBT && currentADBT) || (prevADBT && !currentADBT),
            MessageForReloadSite = (!prevADBT && currentADBT) ? AlphaPlus.Shared.AppResources.Settings.Transfer.Resources.ADBTOnError :
                    (prevADBT && !currentADBT) ? AlphaPlus.Shared.AppResources.Settings.Transfer.Resources.ADBTOffError :
                    string.Empty
        };

        return viewModel;
    }
}