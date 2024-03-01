namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using MediatR;
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class MyWatchlistAccountsQuery : MyWatchlistAccountsQueryBase, IRequest<MyWatchlistAccountsViewModel>
    {
        public MyWatchlistAccountsQuery(IUser viewer) : base(viewer)
        {
            Viewer = viewer;
        }

        public bool IsOpenFromProblemAccount { get; set; }

        public string CustIds { get; set; }

        public IUser Viewer { get; }
    }
}