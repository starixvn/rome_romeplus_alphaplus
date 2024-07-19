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
        var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);
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
            !message.AllYesterdayBalance,
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

    // public async Task<SingleTransferViewModel> Handle(SingleTransferQuery message, CancellationToken cancellationToken)
    // {
    //     var param = new TransferListParam(
    //         message.ViewerId,
    //         message.ViewerLevelId,
    //         message.CustName,
    //         StatusConstant.All,
    //         AutoTransferConstant.All,
    //         false,
    //         message.AllowTotalBalanceTransfer,
    //         1,
    //         1,
    //         message.ActorLevel.IsAgentLevel() ? message.AGIP : string.Empty);
    //     var reportData = await multipleTransferService.GetTransferList(param);

    //     if (reportData.TotalPages == 1)
    //     {
    //         var customer = reportData.Customers.First();

    //         if (customer.CustId == message.CustId)
    //         {
    //             return new SingleTransferViewModel(
    //                 message,
    //                 message.IsYesterday ? customer.YesterdayBalance : customer.Balance);
    //         }
    //     }

    //     return null;
    // }

    // public async Task<Result> Handle(SingleTransferCommand message, CancellationToken cancellationToken)
    // {
    //     var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);

    //     if (isAllDayBalanceTransfer)
    //     {
    //         return Result.CreateErrorResult(Resources.AllowedAllDayBalanceTransferError);
    //     }

    //     var param = new TransferYesterdayBalanceParam(
    //         message.ViewerId,
    //         message.ViewerLevelId,
    //         AppSettings.AllowedListSuperAccounts,
    //         message.CustId.ToString(),
    //         false,
    //         -message.Amount,
    //         string.Empty,
    //         AutoTransferConstant.All,
    //         message.LangKey,
    //         message.IP,
    //         message.SubAccId,
    //         message.AGIP,
    //         isAllDayBalanceTransfer);
    //     return new Result();
    //     // var result = await multipleTransferService.TransferYesterdayBalance(param);

    //     // if (result.IsSuccessful)
    //     // {
    //     //     result.Message = Resources.TransferSuccessfully;
    //     // }

    //     //return result;
    // }

    // public async Task<Result> Handle(MultipleTransferCommand message, CancellationToken cancellationToken)
    // {
    //     var isAllDayBalanceTransfer = transferService.IsAllDayBalanceTransfer(message.ViewerId);

    //     if (isAllDayBalanceTransfer)
    //     {
    //         return Result.CreateErrorResult(Resources.AllowedAllDayBalanceTransferError);
    //     }

    //     var custIds = message.CustIds;
    //     if (string.IsNullOrWhiteSpace(custIds))
    //     {
    //         var transferListParam = new TransferListParam(
    //             message.ViewerId,
    //             message.ViewerLevelId,
    //             message.Username,
    //             message.Status,
    //             message.AutoTransfer,
    //             !message.AllYesterdayBalance,
    //             message.AllowTotalBalanceTransfer,
    //             1,
    //             int.MaxValue - 1,
    //             message.ActorLevel.IsAgentLevel() ? message.AGIP : string.Empty);
    //         var reportData = await multipleTransferService.GetTransferList(transferListParam);
    //         custIds = string.Join(',', reportData.Customers.Select(t => t.CustId));
    //     }

    //     var param = new TransferYesterdayBalanceParam(
    //         message.ViewerId,
    //         message.ViewerLevelId,
    //         AppSettings.AllowedListSuperAccounts,
    //         custIds,
    //         true,
    //         0,
    //         string.Empty,
    //         message.AutoTransfer,
    //         message.LangKey,
    //         message.IP,
    //         message.SubAccId,
    //         message.AGIP,
    //         isAllDayBalanceTransfer);
    //     return new Result();
    //     // var result = await multipleTransferService.TransferYesterdayBalance(param);

    //     // if (result.IsSuccessful)
    //     // {
    //     //     result.Message = Resources.TransferSuccessfully;
    //     // }

    //     //return result;
    // }
}
