using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.ViewModels;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Fanex.Common.Extensions;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.Requests
{
    public class MyThresholdIndexPageQuery : MyThresholdHistoryQuery, IRequest<MyThresholdIndexPageViewModel>
    {
        public MyThresholdIndexPageQuery() : this(null) { }

        public MyThresholdIndexPageQuery(IUser viewer) : base(viewer)
        {
            CurrentDate = DateTime.Today;
            FromDate = CurrentDate.AddDays(-1);
            ToDate = CurrentDate;
            MinDate = CurrentDate.AddMonths(-3).FirstDayOfMonth();
        }

        public DateTime CurrentDate { get; }

        public DateTime MinDate { get; }

        public override bool IsValid()
            => base.IsValid() && FromDate >= MinDate;
    }
}