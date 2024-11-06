using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.Requests
{
    public class MyThresholdHistoryQuery : MemberInfoBaseQuery, IRequest<IEnumerable<ReachHistoryModel>>
    {
        public MyThresholdHistoryQuery() : this(null) { }

        public MyThresholdHistoryQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte SortType { get; set; }

        public bool IsDesc { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate;
    }
}