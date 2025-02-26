using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.MemberRequest;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.MemberRequest
{
    public class MemberRequestsQuery :
        MemberInfoBaseQuery,
        IRequest<MemberRequestViewModel>
    {
        public MemberRequestsQuery() : this(null) { }

        public MemberRequestsQuery(IUser viewer) : base(viewer)
        {
        }

        public int UserSelectedProductId { get; set; }

        public bool DisabledUserSelectedProduct { get; set; }

        public override bool IsValid() => base.IsValid() && CustLevel.IsAgentLevel();
    }
}