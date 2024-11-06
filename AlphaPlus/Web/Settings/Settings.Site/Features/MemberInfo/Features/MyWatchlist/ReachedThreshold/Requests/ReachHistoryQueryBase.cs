using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Fanex.Common.Extensions;
using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.ReachedThreshold.Requests
{
    public class ReachHistoryQueryBase : MemberInfoBaseQuery
    {
        public ReachHistoryQueryBase() : this(null) { }

        public ReachHistoryQueryBase(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
        }

        public int SelectedCustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte LimitType { get; set; }

        public byte SortType { get; set; } = 1;

        public bool IsDesc { get; set; } = true;

        public DateTime LimitedDateToShowInfo { get; }

        public override bool IsValid()
            => base.IsValid() && FromDate <= ToDate && FromDate >= DateTime.Today.AddMonths(-3).FirstDayOfMonth();
    }
}