using Alpha.Main.Models;
using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Requests.Otp
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