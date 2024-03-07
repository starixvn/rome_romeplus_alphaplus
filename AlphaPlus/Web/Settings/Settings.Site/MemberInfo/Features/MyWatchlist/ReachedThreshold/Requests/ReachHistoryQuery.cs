namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Settings.Site.Models;
    using AlphaPlus.Core.Models;

    public class ReachHistoryQuery : ReachHistoryQueryBase, IRequest<ReachHistoryViewModel>
    {
        public ReachHistoryQuery() : this(null){}

        public ReachHistoryQuery(IUser viewer) : base(viewer)
        {
            Viewer = viewer ?? new User(UserSession.Current);
        }

        public IUser Viewer { get; }
    }
}