namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class ForceChangeSecurityCodeQuery : IRequest<ForceChangeSecurityCodeViewModel>
    {
        private readonly IUserSession userSession;

        public ForceChangeSecurityCodeQuery() : this(null)
        {
        }

        public ForceChangeSecurityCodeQuery(IUserSession userSession)
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

        public byte RoleId
        {
            get
            {
                return this.userSession.RoleId;
            }
        }

        public string UserName
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public string Language
        {
            get
            {
                return this.userSession.LangKey;
            }
        }

        public string SubUserName
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }
    }
}