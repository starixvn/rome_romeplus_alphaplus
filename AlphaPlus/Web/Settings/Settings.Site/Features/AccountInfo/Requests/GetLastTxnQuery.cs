using AlphaPlus.Settings.Site.Features.Shared.Requests;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class GetLastTxnQuery : BaseQuery
    {
        public string Username { get; set; }

        public int UplineId => userSession.CustId;

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && UplineId > 0;
    }
}