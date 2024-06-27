namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class UpdateGlobalThresholdCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public UpdateGlobalThresholdCommand() : this(null){}
        
        public UpdateGlobalThresholdCommand(IUser viewer) : base(viewer)
        {
        }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}