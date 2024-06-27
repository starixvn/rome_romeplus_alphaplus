namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using MediatR;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Models;
    using AlphaPlus.Core.Models;

    public class MyWatchlistAccountsQuery : MyWatchlistAccountsQueryBase, IRequest<MyWatchlistAccountsViewModel>
    {
        public MyWatchlistAccountsQuery() : this(null){}
        
        public MyWatchlistAccountsQuery(IUser viewer) : base(viewer)
        {
            Viewer = viewer ?? new User(UserSession.Current);
        }

        public bool IsOpenFromProblemAccount { get; set; }

        public string CustIds { get; set; }

        public IUser Viewer { get; }
    }
}