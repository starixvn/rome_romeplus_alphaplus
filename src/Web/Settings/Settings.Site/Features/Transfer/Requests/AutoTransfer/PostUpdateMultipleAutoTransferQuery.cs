using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    public class PostUpdateMultipleAutoTransferQuery : IRequest<UpdateMultipleAutoTransferResultModel>
    {
        public PostUpdateMultipleAutoTransferQuery() : this(null)
        {
        }

        public PostUpdateMultipleAutoTransferQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
            AGIP = user.AGIP;
            SiteDbGroupId = user.SiteDbGroupId;
        }

        public int UplineId { get; }
        public int SubAccId { get; }
        public string IP { get; }
        public string Language { get; }
        public string AGIP { get; }
        public int SiteDbGroupId { get; }

        public string OnIds { get; set; }
        public string OffIds { get; set; }
        public string OnStatus { get; set; }
        public string OffStatus { get; set; }
        public string OnSchedule { get; set; }
        public string OffSchedule { get; set; }
        public string Old_TransferOption { get; set; }
        public string OnNames { get; set; }
        public string OffNames { get; set; }
    }
}