using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Settings.Transfer.API.Services;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class CashBalanceTransferHandler(
    ICashBalanceTransferService cashBalanceTransferService,
    ITransferService transferService) :
        IRequestHandler<CashBalanceTransferQuery, CashBalanceTransferViewModel>,
        IRequestHandler<CashBalanceTransferCommand, Result>,
        IRequestHandler<HintTransferQuery, HintTransferViewModel>
{
    private readonly ICashBalanceTransferService cashBalanceTransferService = cashBalanceTransferService;
    private readonly ITransferService transferService = transferService;

    public async Task<CashBalanceTransferViewModel> Handle(CashBalanceTransferQuery message, CancellationToken cancellation)
    {
        var model = await cashBalanceTransferService.GetCashBalanceTransfer(message.CustId, message.IsDirectMember, message.AGIP);

        return new CashBalanceTransferViewModel(message.ActorLevel, message.IsDirectMember, model);
    }

    public async Task<Result> Handle(CashBalanceTransferCommand message, CancellationToken cancellation)
    {
        var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ViewerId);
        if (!isAllDayBalTransfer)
        {
            return Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001);
        }

        return await cashBalanceTransferService.AddCashBalanceTransfer(
            message.ViewerId,
            message.CustId,
            message.Amount,
            message.ViewerId,
            message.UplineName,
            message.SubAccId,
            message.IP,
            message.LanguageKey,
            message.AGIP,
            message.IsDirectMember);
    }

    public async Task<HintTransferViewModel> Handle(HintTransferQuery message, CancellationToken cancellation)
    {
        var allowTransfer = message.ActorLevel.IsNotSuperLevel()
            ? await transferService.GetAllowTransfer(message.CustId)
            : string.Empty;
        var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.ViewerId);
        var isEnabledAutoTransfer = !isAllDayBalTransfer && AppSettings.CheckOfferAutoTransferCurrencies(message.Currency);

        return new HintTransferViewModel(allowTransfer, isAllDayBalTransfer, isEnabledAutoTransfer);
    }
}