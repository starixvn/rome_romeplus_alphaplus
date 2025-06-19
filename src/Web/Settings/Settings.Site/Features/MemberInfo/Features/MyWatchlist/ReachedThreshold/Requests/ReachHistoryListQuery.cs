using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachHistoryListQuery : ReachHistoryQueryBase, IRequest<IEnumerable<ReachHistoryModel>>
    {
    }
}