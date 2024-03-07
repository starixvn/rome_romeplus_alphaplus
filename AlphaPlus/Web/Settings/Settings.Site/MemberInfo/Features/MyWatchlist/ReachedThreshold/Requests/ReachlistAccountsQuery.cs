namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ReachlistAccountsQuery : ReachedThresholdQueryBase, IRequest<ReachedThresholdPagingViewModel>
    {
        public ReachlistAccountsQuery() : this(null){}
        
        public ReachlistAccountsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}