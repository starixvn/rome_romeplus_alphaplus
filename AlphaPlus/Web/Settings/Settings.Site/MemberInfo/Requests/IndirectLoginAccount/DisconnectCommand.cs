namespace Alpha.MemberInfo.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class DisconnectCommand : MemberInfoBaseQuery, IRequest<Result>
    {
        public DisconnectCommand(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
            => base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
    }
}