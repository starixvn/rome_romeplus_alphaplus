namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class UpdatePasswordCommand : NewAccountCommand
    {
        public UpdatePasswordCommand() : this(null){}
        
        public UpdatePasswordCommand(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
        }
    }
}