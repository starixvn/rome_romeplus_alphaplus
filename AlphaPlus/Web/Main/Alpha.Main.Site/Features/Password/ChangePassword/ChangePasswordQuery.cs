namespace Alpha.Main.Site.Features.Password
{
    using AlphaPlus.Core.Models;
    using MediatR;

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