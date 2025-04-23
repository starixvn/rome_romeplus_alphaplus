using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    public class NewAccountCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public NewAccountCommand() : this(null) { }

        public NewAccountCommand(IUser viewer) : base(viewer)
        {
        }

        public string Password { get; set; }

        public string BrowserSize { get; set; }

        public override bool IsValid()
            => base.IsValid() && !string.IsNullOrWhiteSpace(Password) && !IsInternal && IsAllowedIndirectLogin;
    }
}