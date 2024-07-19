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
        var model = await cashBalanceTransferService.GetCashBalanceTransfer(message.CustId);

        return new CashBalanceTransferViewModel(
            model.UplineCashBalance,
            model.CustId,
            model.UserName,
            Enumeration.FromValue<UserLevel>(model.RoleId),
            model.CashBalance,
            model.Outstanding,
            model.GivenCredit,
            model.PendingTransfer3rd);
    }

    public async Task<Result> Handle(CashBalanceTransferCommand message, CancellationToken cancellation)
    {
        var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.UplineId);
        if (!isAllDayBalTransfer)
        {
            return Result.CreateErrorResult(Resources.DisallowedAllDayBalanceTransferError, 6001);
        }

        return await cashBalanceTransferService.AddCashBalanceTransfer(
            message.UplineId,
            message.CustId,
            message.Amount,
            message.UplineId,
            message.UplineName,
            message.SubAccId,
            message.IP,
            message.LanguageKey,
            message.AGIP);
    }

    public async Task<HintTransferViewModel> Handle(HintTransferQuery message, CancellationToken cancellation)
    {
        var userRole = Enumeration.FromValue<UserLevel>(message.RoleId);

        var allowTransfer = userRole.IsNotSuperLevel()
            ? await transferService.GetAllowTransfer(message.CustId)
            : string.Empty;
        var isAllDayBalTransfer = await transferService.IsAllDayBalanceTransferAsync(message.CustId);
        var isEnabledAutoTransfer = !isAllDayBalTransfer && AppSettings.CheckOfferAutoTransferCurrencies(message.User.Currency);

        return new HintTransferViewModel(allowTransfer, isAllDayBalTransfer, isEnabledAutoTransfer);
    }
}