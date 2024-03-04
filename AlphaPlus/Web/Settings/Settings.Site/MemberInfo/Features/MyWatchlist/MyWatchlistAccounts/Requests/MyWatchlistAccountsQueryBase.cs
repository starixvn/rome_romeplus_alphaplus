namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using System;

    public class MyWatchlistAccountsQueryBase : MemberInfoBaseQuery
    {
        public MyWatchlistAccountsQueryBase(IUser viewer) : base(viewer)
        {
            IsShowLastLoginColumn = viewer.LimitedDateToShowInfo == default(DateTime);
        }

        public string Username { get; set; }

        public byte SortType { get; set; } = 1;

        public bool IsDesc { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;

        public bool IsShowLastLoginColumn { get; }
    }
}