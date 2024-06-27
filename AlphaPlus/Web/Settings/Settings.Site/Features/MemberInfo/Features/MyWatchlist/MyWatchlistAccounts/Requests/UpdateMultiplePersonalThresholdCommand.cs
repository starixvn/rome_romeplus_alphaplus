namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class UpdateMultiplePersonalThresholdCommand : MemberInfoBaseQuery, IRequest<IEnumerable<NamedResult>>
    {
        public UpdateMultiplePersonalThresholdCommand() : this(null){}
        
        public UpdateMultiplePersonalThresholdCommand(IUser viewer) : base(viewer)
        {
        }
        
        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}