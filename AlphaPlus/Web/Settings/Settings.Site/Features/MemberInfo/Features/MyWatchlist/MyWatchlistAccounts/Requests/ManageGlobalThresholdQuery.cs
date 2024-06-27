namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels;
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ManageGlobalThresholdQuery : MemberInfoBaseQuery, IRequest<ManageGlobalThresholdViewModel>
    {
        public ManageGlobalThresholdQuery() : this(null){}
        
        public ManageGlobalThresholdQuery(IUser viewer) : base(viewer)
        {
        }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }
    }
}