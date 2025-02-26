using AlphaPlus.Settings.Transfer.API.Models.Reward;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;

public class GetRewardPaymentDataQuery : RewardBaseQuery, IRequest<RewardPayment>
{
    public GetRewardPaymentDataQuery()
    {
        IsInternal = viewer.IsInternal;
    }

    public bool IsInternal { get; }
}