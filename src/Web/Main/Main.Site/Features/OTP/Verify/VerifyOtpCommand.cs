using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class VerifyOtpCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public VerifyOtpCommand() : this(null)
        {
        }

        public VerifyOtpCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccountId => userSession.SubAccId;

        public string Otp { get; set; }

        public string Language => userSession.LangKey;

        public string IP => userSession.UserLoginIp;
    }
}