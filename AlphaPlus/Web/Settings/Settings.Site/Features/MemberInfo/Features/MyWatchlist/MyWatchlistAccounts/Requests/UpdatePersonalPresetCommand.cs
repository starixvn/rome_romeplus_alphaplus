namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class UpdatePersonalPresetCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public UpdatePersonalPresetCommand() : this(null){}
        
        public UpdatePersonalPresetCommand(IUser viewer) : base(viewer)
        {
        }

        public int ID { get; set; }

        public string PresetTitle { get; set; }

        public decimal WinningThreshold { get; set; }

        public decimal LosingThreshold { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}