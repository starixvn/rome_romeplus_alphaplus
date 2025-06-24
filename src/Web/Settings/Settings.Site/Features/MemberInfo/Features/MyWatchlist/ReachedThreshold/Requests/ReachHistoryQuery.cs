using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachHistoryQuery : ReachHistoryQueryBase, IRequest<ReachHistoryViewModel>
    {
    }
}