namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class ResetSecurityCodeCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ResetSecurityCodeCommand() : this(null)
        {
        }

        public ResetSecurityCodeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int? CustId { get; set; }

        public int UserId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public int? SubAccId { get; set; }

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