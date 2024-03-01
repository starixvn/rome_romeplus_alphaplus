namespace AlphaPlus.Settings.Site.Requests.IndirectLoginAccount
{
    using AlphaPlus.Settings.Site.ViewModels.IndirectLoginAccount;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class ManageAccountQuery : MemberInfoBaseQuery, IRequest<ManageAccountViewModel>
    {

        public ManageAccountQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && IsAllowedIndirectLogin;
        }
    }
}