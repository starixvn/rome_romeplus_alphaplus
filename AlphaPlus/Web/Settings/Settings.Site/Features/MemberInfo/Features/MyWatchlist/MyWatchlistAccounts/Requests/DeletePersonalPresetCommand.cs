namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Core.Models;

    public class DeletePersonalPresetCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public DeletePersonalPresetCommand() : this(null) { }

        public DeletePersonalPresetCommand(IUser viewer) : base(viewer)
        {
        }

        public int ID { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}