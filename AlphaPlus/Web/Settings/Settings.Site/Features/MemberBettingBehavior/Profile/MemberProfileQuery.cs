using AlphaPlus.Settings.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberBettingBehavior.Profile
{
    public class MemberProfileQuery : BaseQuery, IRequest<CustomerProfile>
    {
        public int MemberCustId { get; set; }

        public string UserName { get; set; }
    }
}