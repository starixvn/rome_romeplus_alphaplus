using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.Requests
{
    public class MyThresholdHistoryQuery : MemberInfoBaseQuery, IRequest<IEnumerable<ReachHistoryModel>>
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte SortType { get; set; }

        public bool IsDesc { get; set; }

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate;
    }
}