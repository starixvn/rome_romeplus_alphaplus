using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.ViewModels;
using Fanex.Common.Extensions;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyThresholds.Requests
{
    public class MyThresholdIndexPageQuery : MyThresholdHistoryQuery, IRequest<MyThresholdIndexPageViewModel>
    {
        public MyThresholdIndexPageQuery()
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