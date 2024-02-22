namespace AlphaPlus.Main.Site.Features.OTP
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class ResetOtpCommand : IRequest<ResetOtpViewModel>
    {
        private readonly IUserSession userSession;

        public ResetOtpCommand() : this(null)
        {
        }

        public ResetOtpCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string Username { get; set; }

        public int ActorId
        {
            get
            {
                return this.userSession.UserId;
            }
        }

        public string Language
        {
            get
            {
                return this.userSession.LangKey;
            }
        }
    }
}