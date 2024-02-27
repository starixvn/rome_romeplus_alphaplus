namespace AlphaPlus.Main.Site.Features.OTP
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class RemoveOtpCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public RemoveOtpCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public int SubAccountId
        {
            get
            {
                return userSession.SubAccId;
            }
        }

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