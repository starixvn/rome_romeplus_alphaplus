using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    public class CancelJobQuery : IRequest<Result>
    {
        public CancelJobQuery() : this(null)
        {
        }

        public CancelJobQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);

            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            SiteDbGroupId = user.SiteDbGroupId;
        }

        public int CustId { get; set; }

        public string ViewStatus { get; set; }

        public int UplineId { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public int SiteDbGroupId { get; }

        public bool IsValid() => CustId > 0 && UplineId > 0;
    }
}