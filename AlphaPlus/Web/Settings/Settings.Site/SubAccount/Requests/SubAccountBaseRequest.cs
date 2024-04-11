using AlphaPlus.Core.Models;

namespace AlphaPlus.Settings.Site.SubAccount.Requests
{
    public abstract class SubAccountBaseRequest
    {
        public int SubAccId { get; set; } = UserSession.Current.SubAccId;

        public virtual bool IsValid() => SubAccId > 0;
    }
}