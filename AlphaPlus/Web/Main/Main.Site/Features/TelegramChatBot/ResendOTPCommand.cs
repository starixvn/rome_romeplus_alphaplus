using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.TelegramChatBot
{
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