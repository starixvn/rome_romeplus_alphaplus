namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class ChangeUserOwnSecurityCodeCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ChangeUserOwnSecurityCodeCommand() : this(null)
        {
        }

        public ChangeUserOwnSecurityCodeCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int UserId => userSession.UserId;

        public int SubAccId => userSession.SubAccId;

        public bool IsSubAcc => userSession.IsSubAcc;

        public string OldSecurityCode { get; set; }

        public string SecurityCode { get; set; }

        public string SecurityCodeToken => userSession.SecurityCodeToken;

        public string Language => userSession.Language;

        public string IP => userSession.UserLoginIp;
    }
}