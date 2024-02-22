using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.TelegramChatBot
{
    public class SetUpChatBotQuery : IRequest<ChatBotViewModel>
    {
        public SetUpChatBotQuery() : this(null)
        {
        }

        public SetUpChatBotQuery(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            CustId = userSession.CustId;
            CustLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
        }

        public int CustId { get; }

        public UserLevel CustLevel { get; }

        public int SubAccId { get; }

        public string Language { get; }
    }
}