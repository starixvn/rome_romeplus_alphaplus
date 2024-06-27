namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using System;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
    using AlphaPlus.Core.Models;

    public class MyWatchlistAccountsQueryBase : MemberInfoBaseQuery
    {
        public MyWatchlistAccountsQueryBase() : this(null){}

        public MyWatchlistAccountsQueryBase(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
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