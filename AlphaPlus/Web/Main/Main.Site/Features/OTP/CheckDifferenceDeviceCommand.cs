using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class CheckDifferenceDeviceCommand : IRequest<bool>
    {
        private readonly IUserSession userSession;

        public CheckDifferenceDeviceCommand() : this(null)
        {
        }

        public CheckDifferenceDeviceCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public int SubAccountId
        {
            get
            {
                return userSession.SubAccId;
            }
        }
    }
}