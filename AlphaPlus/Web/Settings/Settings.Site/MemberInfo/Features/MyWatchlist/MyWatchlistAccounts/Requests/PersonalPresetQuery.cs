namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests
{
    using AlphaPlus.Settings.Site.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class PersonalPresetQuery : MemberInfoBaseQuery, IRequest<IEnumerable<PersonalPresetModel>>
    {
        public PersonalPresetQuery(IUser viewer) : base(viewer)
        {
        }
    }
}