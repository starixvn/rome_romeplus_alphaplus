using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    public class ValidateAccountQuery : MemberInfoBaseQuery, IRequest<Result>
    {
        public ValidateAccountQuery() : this(null) { }

        public ValidateAccountQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
        }
    }
}