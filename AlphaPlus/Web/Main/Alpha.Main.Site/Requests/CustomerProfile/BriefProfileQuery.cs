using AlphaPlus.Core.Models;
using MediatR;
using Alpha.Main.Site.ViewModels.CustomerProfile;
using AlphaPlus.SecurityCode.Web;

namespace Alpha.Main.Site.Models.Requests.CustomerProfile
{
    public class BriefProfileQuery : IRequest<BriefProfileViewModel>
    {
        private readonly IUserSession userSession;

        public BriefProfileQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public string UserName
        {
            get
            {
                return this.userSession.UserName;
            }
        }

        public string NickName
        {
            get
            {
                return this.userSession.NickName;
            }
        }

        public bool IsSubAcc
        {
            get
            {
                return this.userSession.IsSubAcc;
            }
        }

        public bool IsInternal
        {
            get
            {
                return this.userSession.IsInternal;
            }
        }

        public string SubUserName
        {
            get
            {
                return this.userSession.SubUserName;
            }
        }

        public string AdminName
        {
            get
            {
                return this.userSession.AdminName;
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