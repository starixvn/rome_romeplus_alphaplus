namespace AlphaPlus.Main.Site.Features.TelegramChatBot
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class VerifyOTPCommand : IRequest<Result>
    {
        public VerifyOTPCommand() : this(null)
        {
        }

        public VerifyOTPCommand(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Username = userSession.UserName;
            SubUserName = userSession.SubUserName;
            IsSubAcc = userSession.IsSubAcc;
            IP = userSession.UserLoginIp;
        }

        public string OTP { get; set; }

        public bool IsShowCaptcha { get; set; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Username { get; }

        public string SubUserName { get; }

        public bool IsSubAcc { get; }

        public string IP { get; }
    }
}