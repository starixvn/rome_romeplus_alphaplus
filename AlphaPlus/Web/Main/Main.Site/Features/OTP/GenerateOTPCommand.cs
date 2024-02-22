using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class GenerateOTPCommand : IRequest<Result>
    {
        public GenerateOTPCommand(IUserSession userSession)
        {
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int CustId { get; }

        public int SubAccId { get; }
    }
}