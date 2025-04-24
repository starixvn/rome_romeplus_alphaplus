using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class MyWatchlistAccountsQueryBase : MemberInfoBaseQuery
    {
        public DateTime FromDate { get; set; } = DateTime.Today.FirstDayOfLastMonth();

        public DateTime ToDate { get; set; } = DateTime.Today;

        public byte Status { get; set; }

        public byte ProblemAccount { get; set; } = 2;

        public string Username { get; set; }

        public byte SortType { get; set; } = 1;

        public bool IsDesc { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;

        public bool IsShowLastLoginColumn => viewer.LimitedDateToShowInfo == default;
    }
}