namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class MyThresholdQuery : MemberInfoBaseQuery, IRequest<ThresholdViewModel>
    {
        public MyThresholdQuery() : this(null){}
        
        public MyThresholdQuery(IUser viewer) : base(viewer)
        {
        }
    }
}