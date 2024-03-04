namespace AlphaPlus.Settings.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class ReachHistoryListQuery : ReachHistoryQueryBase, IRequest<IEnumerable<ReachHistoryModel>>
    {
        public ReachHistoryListQuery(IUser viewer) : base(viewer)
        {
        }
    }
}