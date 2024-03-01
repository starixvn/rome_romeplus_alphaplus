namespace Alpha.MemberInfo.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using Alpha.MemberInfo.Site.Features.MyWatchlist.ReachedThreshold.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ReachlistAccountsQuery : ReachedThresholdQueryBase, IRequest<ReachedThresholdPagingViewModel>
    {
        public ReachlistAccountsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}