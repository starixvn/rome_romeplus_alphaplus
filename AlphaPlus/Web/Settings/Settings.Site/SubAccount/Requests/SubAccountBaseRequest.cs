using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public abstract class SubAccountBaseRequest
    {
        public int CustId => UserSession.Current.CustId;

        public int SubAccId { get; set; } = UserSession.Current.SubAccId;

        public string IP => UserSession.Current.UserLoginIp;

        public string Language => UserSession.Current.LangKey;

        public virtual bool IsValid() => SubAccId > 0;
    }
}