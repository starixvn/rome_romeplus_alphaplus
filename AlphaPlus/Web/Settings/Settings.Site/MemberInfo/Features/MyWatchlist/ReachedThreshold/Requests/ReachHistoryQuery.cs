namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ReachHistoryQuery : ReachHistoryQueryBase, IRequest<ReachHistoryViewModel>
    {
        public ReachHistoryQuery(IUser viewer) : base(viewer)
        {
            Viewer = viewer;
        }

        public IUser Viewer { get; }
    }
}