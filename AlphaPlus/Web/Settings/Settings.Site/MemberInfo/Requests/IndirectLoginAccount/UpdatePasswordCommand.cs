namespace AlphaPlus.Settings.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;

    public class UpdatePasswordCommand : NewAccountCommand
    {
        public UpdatePasswordCommand(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
        }
    }
}