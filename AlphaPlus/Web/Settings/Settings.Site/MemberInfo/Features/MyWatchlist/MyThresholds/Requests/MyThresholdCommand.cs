namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyThresholds.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Settings.Site.Requests;

    public class MyThresholdCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public MyThresholdCommand() : this(null){}
        
        public MyThresholdCommand(IUser viewer) : base(viewer)
        {
        }

        public decimal? WinningThreshold { get; set; }

        public decimal? LosingThreshold { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal;
        }
    }
}