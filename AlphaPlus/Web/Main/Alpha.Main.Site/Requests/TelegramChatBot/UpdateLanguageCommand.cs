namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using AlphaPlus.Core.Models;
    using MediatR;
    using TelegramBot.Client.Models;

    public class UpdateLanguageCommand : IRequest<Result>
    {
        public UpdateLanguageCommand() : this(null)
        {
        }

        public UpdateLanguageCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            UserLanguage = userSession.Language;
            IP = userSession.UserLoginIp;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public string UserLanguage { get; }

        public string Language { get; set; }

        public string IP { get; }
    }
}