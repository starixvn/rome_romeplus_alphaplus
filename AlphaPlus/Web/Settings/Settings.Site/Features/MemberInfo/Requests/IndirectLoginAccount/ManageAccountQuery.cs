using AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.IndirectLoginAccount;
using AlphaPlus.Settings.MemberInfo.Domain.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Requests.IndirectLoginAccount
{
    public class ManageAccountQuery : MemberInfoBaseQuery, IRequest<ManageAccountViewModel>
    {
        public ManageAccountQuery() : this(null) { }

        public ManageAccountQuery(IUser viewer) : base(viewer)
        {
        }

        public override bool IsValid()
        {
            return base.IsValid() && IsAllowedIndirectLogin;
        }
    }
}