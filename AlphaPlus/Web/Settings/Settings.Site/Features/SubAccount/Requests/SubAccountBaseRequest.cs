using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.Features.SubAccount.Requests
{
    public abstract class SubAccountBaseRequest
    {
        public int CustId => UserSession.Current.CustId;

        public int SubAccId { get; set; } = UserSession.Current.SubAccId;

        public string IP => UserSession.Current.UserLoginIp;

        public string Language => UserSession.Current.LangKey;

        public bool HasUpdatePermission => !UserSession.Current.IsLimitedHostPermissionOnSubAccountFunction;

        public virtual bool IsValid() => CustId > 0;
    }
}