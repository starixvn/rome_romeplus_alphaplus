using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    public class GetTodayTransfersQuery : IRequest<AutoTransferMonitoringPageModel>
    {
        public GetTodayTransfersQuery() : this(null)
        {
        }

        public GetTodayTransfersQuery(IUser user)
        {
            user ??= new User(UserSession.Current);
            CustId = user.Id;
            Language = user.LanguageKey;
            AGIP = user.AGIP;
            SiteDbGroupId = user.SiteDbGroupId;
        }

        public int CustId { get; }

        public string Language { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public byte LevelType { get; set; }

        public string AGIP { get; }
        public int SiteDbGroupId { get; }

        public bool IsValid() => CustId > 0;
    }
}