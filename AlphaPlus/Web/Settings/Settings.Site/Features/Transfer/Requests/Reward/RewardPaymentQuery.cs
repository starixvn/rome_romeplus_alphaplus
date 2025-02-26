using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;

public class RewardPaymentQuery : RewardBaseQuery, IRequest<RewardPaymentViewModel>
{
}