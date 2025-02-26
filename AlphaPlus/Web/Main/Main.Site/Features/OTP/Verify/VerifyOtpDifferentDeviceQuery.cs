using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.OTP
{
    public class VerifyOtpDifferentDeviceQuery : IRequest<VerifyOtpDifferenceDeviceViewModel>
    {
        private readonly IUserSession userSession;

        public VerifyOtpDifferentDeviceQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public bool IsSubAcc => userSession.IsSubAcc;

        public string UserName => userSession.UserName;

        public string NickName => userSession.NickName;

        public string Language => userSession.Language;

        public string SubUserName => userSession.SubUserName;
    }
}