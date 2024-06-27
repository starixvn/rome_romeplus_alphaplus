namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class DisconnectCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public DisconnectCommand() : this(null){}
        
        public DisconnectCommand(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
            => base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
    }
}