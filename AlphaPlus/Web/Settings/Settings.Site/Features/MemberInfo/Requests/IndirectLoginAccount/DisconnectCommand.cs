using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    public class DisconnectCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public DisconnectCommand() : this(null) { }

        public DisconnectCommand(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
            => base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
    }
}