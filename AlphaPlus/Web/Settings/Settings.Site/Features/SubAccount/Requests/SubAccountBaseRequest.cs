using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Requests
{
    public abstract class SubAccountBaseRequest
    {
        protected readonly IUserSession userSession;
        public SubAccountBaseRequest() : this(null)
        {
        }

        internal SubAccountBaseRequest(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string IP => userSession.UserLoginIp;

        public string Language => userSession.LangKey;

        public bool HasUpdatePermission => !userSession.IsLimitedHostPermissionOnSubAccountFunction;

        public virtual bool IsValid() => CustId > 0;
    }
}