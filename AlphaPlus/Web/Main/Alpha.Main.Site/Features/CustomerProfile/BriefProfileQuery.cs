using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.SecurityCode.Web;

namespace Alpha.Main.Site.Features.CustomerProfile
{
    public class BriefProfileQuery(IUserSession userSession) : IRequest<BriefProfileViewModel>
    {
        private readonly IUserSession userSession = userSession;

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public string UserName
        {
            get
            {
                return userSession.UserName;
            }
        }

        public string NickName
        {
            get
            {
                return userSession.NickName;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return userSession.IsSubAcc;
            }
        }

        public bool IsInternal
        {
            get
            {
                return userSession.IsInternal;
            }
        }

        public string SubUserName
        {
            get
            {
                return userSession.SubUserName;
            }
        }

        public string AdminName
        {
            get
            {
                return userSession.AdminName;
            }
        }

        public bool IsEnabledOtp
        {
            get
            {
                return SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp;
            }
        }

        public bool IsProtectedMode => userSession.IsProtectedMode;

        public int SubAccId => userSession.SubAccId;
    }
}