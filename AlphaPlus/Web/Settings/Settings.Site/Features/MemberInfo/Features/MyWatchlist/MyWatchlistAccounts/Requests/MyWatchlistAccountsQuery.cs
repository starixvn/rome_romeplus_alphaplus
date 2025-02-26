using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class MyWatchlistAccountsQuery : MyWatchlistAccountsQueryBase, IRequest<MyWatchlistAccountsViewModel>
    {
    }
}