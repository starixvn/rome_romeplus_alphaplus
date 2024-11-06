using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.Requests
{
    public class MyThresholdQuery : MemberInfoBaseQuery, IRequest<ThresholdViewModel>
    {
        public MyThresholdQuery() : this(null) { }

        public MyThresholdQuery(IUser viewer) : base(viewer)
        {
        }
    }
}