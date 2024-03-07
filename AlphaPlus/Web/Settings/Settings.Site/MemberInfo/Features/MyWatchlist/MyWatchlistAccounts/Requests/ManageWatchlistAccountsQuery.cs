namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ManageWatchlistAccountsQuery : MemberInfoBaseQuery, IRequest<ManageWatchlistAccountsViewModel>
    {
        public ManageWatchlistAccountsQuery() : this(null){}
        
        public ManageWatchlistAccountsQuery(IUser viewer) : base(viewer)
        {
        }

        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }

        public bool IsEdit { get; set; }
    }
}