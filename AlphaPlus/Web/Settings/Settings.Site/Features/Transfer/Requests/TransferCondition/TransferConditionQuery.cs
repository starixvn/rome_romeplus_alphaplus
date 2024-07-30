using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferCondition;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferCondition;

public class TransferConditionQuery : TransferBaseQuery, IRequest<TransferConditionViewModel>
{
}
