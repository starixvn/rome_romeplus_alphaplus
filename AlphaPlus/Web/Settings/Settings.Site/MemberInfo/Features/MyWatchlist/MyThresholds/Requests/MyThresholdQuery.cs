namespace Alpha.MemberInfo.Site.Features.MyWatchlist.MyThresholds.Requests
{
    using Alpha.MemberInfo.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using Alpha.MemberInfo.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class MyThresholdQuery : MemberInfoBaseQuery, IRequest<ThresholdViewModel>
    {
        public MyThresholdQuery(IUser viewer) : base(viewer)
        {
        }
    }
}