using AlphaPlus.Settings.Site.Features.Shared.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class OnlineListQuery : BaseQuery
    {
        public virtual int SearchAccountId { get; set; }

        public override bool IsValid() => SearchAccountId > 0 && !SearchAccountId.Equals(userSession.CustId);
    }
}