using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.Password
{
    public class ChangePasswordQuery : IRequest<PasswordViewModel>
    {
        private readonly IUserSession userSession;

        public ChangePasswordQuery() : this(null)
        {
        }

        internal ChangePasswordQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public byte CustRoleId
        {
            get
            {
                return this.userSession.RoleId;
            }
        }

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;
    }
}