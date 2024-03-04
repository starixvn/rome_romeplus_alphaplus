namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class WatchlistAccountsQuery : MyWatchlistAccountsQueryBase, IRequest<MyWatchlistAccountsPagingViewModel>
    {
        public WatchlistAccountsQuery(IUser viewer) : base(viewer)
        {
        }
    }
}