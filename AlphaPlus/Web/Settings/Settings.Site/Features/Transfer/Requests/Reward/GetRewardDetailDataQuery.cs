using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.Reward;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.Reward;

public class GetRewardDetailDataQuery : RewardBaseQuery, IRequest<RewardDetailDataViewModel>
{
    public GetRewardDetailDataQuery()
    {
        IsInternal = viewer.IsInternal;
    }

    public bool IsInternal { get; }
}