using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    public class PostUpdateScheduleQuery : IRequest<Result>
    {
        public PostUpdateScheduleQuery() : this(null)
        {
        }

        public PostUpdateScheduleQuery(IUser user)
        {
            user ??= new User(UserSession.Current);
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
            SiteDbGroupId = user.SiteDbGroupId;
        }

        public int CustId { get; set; }
        public string CustName { get; set; }
        public string Schedule { get; set; }
        public string Old_TransferOption { get; set; }

        public string Language { get; }
        public int UplineId { get; }
        public int SubAccId { get; }
        public string IP { get; }
        public int SiteDbGroupId { get; }

        public string[] ScheduleDates => (Schedule ?? string.Empty).Split([','], StringSplitOptions.RemoveEmptyEntries);

        public bool IsValid() => CustId > 0 && UplineId > 0 && ScheduleDates.Any();
    }
}