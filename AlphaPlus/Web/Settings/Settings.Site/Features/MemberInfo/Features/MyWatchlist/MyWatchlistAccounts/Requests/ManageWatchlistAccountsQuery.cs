using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class ManageWatchlistAccountsQuery : MemberInfoBaseQuery, IRequest<ManageWatchlistAccountsViewModel>
    {
        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }

        public bool IsEdit { get; set; }

        public bool MyWatchlistPage { get; set; }
    }
}