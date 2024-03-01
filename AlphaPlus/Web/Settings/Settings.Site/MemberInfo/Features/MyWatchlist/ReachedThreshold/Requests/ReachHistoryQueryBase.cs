namespace Alpha.MemberInfo.Site.Features.MyWatchlist.ReachedThreshold.Requests
{
    using Alpha.MemberInfo.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using Fanex.Common.Extensions;
    using System;

    public class ReachHistoryQueryBase : MemberInfoBaseQuery
    {
        public ReachHistoryQueryBase(IUser viewer) : base(viewer)
        {
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