using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachlistAccountsQuery : ReachedThresholdQueryBase, IRequest<ReachedThresholdPagingViewModel>
    {
        public ReachlistAccountsQuery() : this(null) { }

        public ReachlistAccountsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}