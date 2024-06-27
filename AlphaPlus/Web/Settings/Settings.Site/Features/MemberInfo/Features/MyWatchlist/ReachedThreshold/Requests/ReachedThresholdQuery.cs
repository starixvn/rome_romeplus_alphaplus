namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
    using AlphaPlus.Core.Models;

    public class ReachedThresholdQuery : ReachedThresholdQueryBase, IRequest<ReachedThresholdViewModel>
    {
        public ReachedThresholdQuery() : this(null){}

        public ReachedThresholdQuery(IUser viewer) : base(viewer)
        {
            Viewer = viewer ?? new User(UserSession.Current);
        }

        public IUser Viewer { get; }
    }
}