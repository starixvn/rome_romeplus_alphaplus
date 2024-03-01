namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class DeleteMultiplePersonalThresholdCommand : MemberInfoBaseQuery, IRequest<IEnumerable<NamedResult>>
    {
        public DeleteMultiplePersonalThresholdCommand(IUser viewer) : base(viewer)
        {
        }

        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}