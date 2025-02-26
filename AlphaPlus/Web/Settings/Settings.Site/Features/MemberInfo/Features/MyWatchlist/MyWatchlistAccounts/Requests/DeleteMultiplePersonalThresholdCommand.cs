using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    public class DeleteMultiplePersonalThresholdCommand : MemberInfoBaseQuery, IRequest<IEnumerable<NamedResult>>
    {
        public string CustIds { get; set; }

        public string Usernames { get; set; }

        public override bool IsValid()
            => base.IsValid() && !IsInternal;
    }
}