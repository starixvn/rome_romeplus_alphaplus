namespace AlphaPlus.Main.Site.Features.OTP
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class ChangeOtpStatusCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public ChangeOtpStatusCommand() : this(null)
        {
        }

        public ChangeOtpStatusCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public bool IsEnable { get; set; }

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
                return this.userSession.Language;
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