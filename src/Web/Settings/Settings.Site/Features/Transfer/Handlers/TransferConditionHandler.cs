using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferCondition;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferCondition;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;
using AlphaPlus.Settings.Transfer.API.Configurations;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Params.TransferCondition;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferConditionHandler(ITransferService transferService) :
    IRequestHandler<TransferConditionQuery, TransferConditionViewModel>,
    IRequestHandler<UpdateTransferConditionCommand, Result>
{
    public async Task<TransferConditionViewModel> Handle(TransferConditionQuery message, CancellationToken cancellationToken)
    {
        var isEnableAutoTransfer = AppSettings.CheckOfferAutoTransferCurrencies(message.Site);
        var superIdTask = transferService.GetSuperIdAutomateTransfer(message.CustId);
        var transferOptionTask = transferService.GetTransferOption(message.CustId);
        await Task.WhenAll(superIdTask, transferOptionTask);

        var warningMessage = string.Empty;

        if (await superIdTask > 0)
        {
            warningMessage = message.ActorLevel.IsSuperLevel()
                ? Resources.TransferConditionByCompany
                : Resources.TransferConditionByUpline;
        }

        return new TransferConditionViewModel(
            message.CustId,
            await transferOptionTask,
            warningMessage,
            isEnableAutoTransfer);
    }

    public Task<Result> Handle(UpdateTransferConditionCommand message, CancellationToken cancellationToken)
        => transferService.UpdateTransferOption(new TransferConditionParam(
            message.ViewerId,
            message.ViewerLevelId,
            message.CustId,
            message.TransferOption,
            message.AutoTransferOption,
            message.SubAccId,
            message.IP,
            message.LangKey,
            message.AGIP));
}