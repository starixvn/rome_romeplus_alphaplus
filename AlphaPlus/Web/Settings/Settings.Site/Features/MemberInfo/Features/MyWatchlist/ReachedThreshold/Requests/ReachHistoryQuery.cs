using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachHistoryQuery : ReachHistoryQueryBase, IRequest<ReachHistoryViewModel>
    {
        public ReachHistoryQuery() : this(null) { }

        public ReachHistoryQuery(IUser viewer) : base(viewer)
        {
            Viewer = viewer ?? new User(UserSession.Current);
        }

        public IUser Viewer { get; }
    }
}