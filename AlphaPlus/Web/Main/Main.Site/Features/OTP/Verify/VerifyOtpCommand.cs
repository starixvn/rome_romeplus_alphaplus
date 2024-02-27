namespace AlphaPlus.Main.Site.Features.OTP
{
    using AlphaPlus.Core.Models;
    using MediatR;

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

        public string Otp { get; set; }

        public string Language
        {
            get
            {
                return this.userSession.LangKey;
            }
        }
    }
}