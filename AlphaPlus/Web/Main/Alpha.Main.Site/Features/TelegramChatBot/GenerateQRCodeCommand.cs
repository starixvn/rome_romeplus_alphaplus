using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.TelegramChatBot
{
    public class GenerateQRCodeCommand : IRequest<VerifyAccountViewModel>
    {
        public GenerateQRCodeCommand() : this(null)
        {
        }

        public GenerateQRCodeCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustomerLevel = userSession.RoleId;
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            Language = userSession.Language;
        }

        public int CustomerLevel { get; }

        public int CustId { get; }

        public int SubAccId { get; }

        public string Language { get; }
    }
}