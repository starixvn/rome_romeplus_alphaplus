namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class DeletePersonalPresetCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public DeletePersonalPresetCommand(IUser viewer) : base(viewer)
        {
        }

        public int ID { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}