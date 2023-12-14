namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using AlphaPlus.Core.Models;
    using MediatR;
    using TelegramBot.Client.Models;

    public class ResendOTPCommand : IRequest<Result>
    {
        public ResendOTPCommand() : this(null)
        {
        }

        public ResendOTPCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }
    }
}