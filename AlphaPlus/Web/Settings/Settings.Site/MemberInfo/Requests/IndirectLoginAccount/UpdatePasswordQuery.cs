namespace AlphaPlus.Settings.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.Site.ViewModels.IndirectLoginAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class UpdatePasswordQuery : MemberInfoBaseQuery, IRequest<UpdatePasswordViewModel>
    {
        public UpdatePasswordQuery() : this(null){}
        
        public UpdatePasswordQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && !IsInternal && IsAllowedIndirectLogin;
        }
    }
}