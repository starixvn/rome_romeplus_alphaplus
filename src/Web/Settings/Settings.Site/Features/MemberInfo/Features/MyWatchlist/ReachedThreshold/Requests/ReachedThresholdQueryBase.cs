using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using Fanex.Common.Extensions;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachedThresholdQueryBase : MemberInfoBaseQuery
    {

        public string Username { get; set; }

        public DateTime FromDate { get; set; } = DateTime.Today.AddDays(-1);

        public DateTime ToDate { get; set; } = DateTime.Today;

        public byte Status { get; set; }

        public byte LimitType { get; set; }

        public byte SortType { get; set; } = 1;

        public bool IsDesc { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;

        public DateTime MinDate => DateTime.Today.AddMonths(-3).FirstDayOfMonth();

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate && FromDate >= MinDate;
    }
}