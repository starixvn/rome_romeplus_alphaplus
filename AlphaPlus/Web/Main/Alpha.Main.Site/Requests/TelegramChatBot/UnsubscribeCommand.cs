namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using AlphaPlus.Core.Models;
    using MediatR;
    using TelegramBot.Client.Models;

    public class UnsubscribeCommand : IRequest<Result>
    {
        public UnsubscribeCommand() : this(null)
        {
        }

        public UnsubscribeCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            IP = userSession.UserLoginIp;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string IP { get; }
    }
}