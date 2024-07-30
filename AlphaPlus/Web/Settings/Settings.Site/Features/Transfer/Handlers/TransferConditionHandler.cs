using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferCondition;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferCondition;
using AlphaPlus.Settings.Transfer.API.Services;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Handlers;

public class TransferConditionHandler(ITransferService transferService) : IRequestHandler<TransferConditionQuery, TransferConditionViewModel>
{
    public async Task<TransferConditionViewModel> Handle(TransferConditionQuery message, CancellationToken cancellationToken)
    {
        var transferCondition = await transferService.GetTransferOption(message.CustId);

        return new TransferConditionViewModel(message, transferCondition);
    }
}