using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    public class ChangeSecurityCodeFirstLoginCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ChangeSecurityCodeFirstLoginCommand() : this(null)
        {
        }

        public ChangeSecurityCodeFirstLoginCommand(IUserSession userSession)
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

        public int UserId
        {
            get
            {
                return this.userSession.UserId;
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

        public string SecurityCode { get; set; }

        public string Language
        {
            get
            {
                return this.userSession.LangKey;
            }
        }

        public string IP
        {
            get
            {
                return this.userSession.UserLoginIp;
            }
        }
    }
}