namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class SecurityCodeQuery : IRequest<SecurityCodeViewModel>
    {
        private readonly IUserSession userSession;

        public SecurityCodeQuery() : this(null)
        {
        }

        public SecurityCodeQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public byte RoleId
        {
            get
            {
                return this.userSession.RoleId;
            }
        }

        public int SubAccId
        {
            get
            {
                return this.userSession.SubAccId;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public string SubUserName
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }

        public string Username { get; set; }

        public bool IsEnabledSecurityCodeInBetLists => userSession.IsEnabledSecurityCodeInBetLists;

        public bool IsEnabledSecurityCodeInStatement => userSession.IsEnabledSecurityCodeInStatement;

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;
    }
}