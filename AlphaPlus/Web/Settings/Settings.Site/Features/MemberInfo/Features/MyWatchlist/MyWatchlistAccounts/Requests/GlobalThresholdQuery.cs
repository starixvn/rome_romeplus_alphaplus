namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class GlobalThresholdQuery : MemberInfoBaseQuery, IRequest<GlobalThresholdModel>
    {
        public GlobalThresholdQuery() : this(null){}
        
        public GlobalThresholdQuery(IUser viewer) : base (viewer)
        {
        }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }
    }
}