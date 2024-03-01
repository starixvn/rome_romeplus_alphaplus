namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests
{
    using System;
    using System.Collections.Generic;
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.ReachedThreshold.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class MyThresholdHistoryQuery : MemberInfoBaseQuery, IRequest<IEnumerable<ReachHistoryModel>>
    {
        public MyThresholdHistoryQuery(IUser viewer) : base(viewer)
        {
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