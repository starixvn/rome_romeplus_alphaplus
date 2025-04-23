using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferAllAndClearCredit;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferAllAndClearCredit;
using AlphaPlus.Settings.Transfer.API.Models.TransferAllAndClearCredit;
using AlphaPlus.Settings.Transfer.API.Params.TransferAllAndClearCredit;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferAllAndClearCreditHandler(
    TransferAllAndClearCreditService transferAllAndClearCreditService,
    ITransferService transferService) :
        IRequestHandler<CalculationQuery, CalculationViewModel>,
        IRequestHandler<TransferResultQuery, TransferResultViewModel>
{
    private readonly TransferAllAndClearCreditService transferAllAndClearCreditService = transferAllAndClearCreditService;
    private readonly ITransferService transferService = transferService;

    public async Task<CalculationViewModel> Handle(CalculationQuery message, CancellationToken cancellation)
    {
        var calculationResult = new TransferResult();

        if (!string.IsNullOrWhiteSpace(message.Username))
        {
            var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransfer(message.ViewerId);
            var param = new GetCalculationResultParam(
                message.CustId,
                message.Username,
                message.LanguageKey,
                isAllDayBalanceTransfer,
                message.AGIP);
            calculationResult = transferAllAndClearCreditService.GetCalculateResult(param);
        }

        return new CalculationViewModel(message, calculationResult);
    }

    public async Task<TransferResultViewModel> Handle(TransferResultQuery message, CancellationToken cancellation)
    {
        var isAllDayBalanceTransfer = await transferService.IsAllDayBalanceTransfer(message.ViewerId);
        var param = new GetTransferResultParam(
            message.CustId,
            message.UserId,
            message.SubAccId,
            message.IsClearCredit,
            message.Username,
            message.IP,
            message.LanguageKey,
            message.TransferSteps,
            message.ClearCreditSteps,
            isAllDayBalanceTransfer,
            message.AGIP);

        var transferResult = message.IsInternal ? null : transferAllAndClearCreditService.GetTransferResult(param);

        return new TransferResultViewModel(message, param, transferResult);
    }
}