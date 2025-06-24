using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using Fanex.Common.Extensions;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachHistoryQueryBase : MemberInfoBaseQuery
    {
        public int SelectedCustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte LimitType { get; set; }

        public byte SortType { get; set; } = 1;

        public bool IsDesc { get; set; } = true;

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate && FromDate >= DateTime.Today.AddMonths(-3).FirstDayOfMonth();
    }
}