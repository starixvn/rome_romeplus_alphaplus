using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Helpers;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Shared.AppResources.Settings.Transfer;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    public class UpdateQuery : IRequest<Result>
    {
        public UpdateQuery() : this(null)
        {
        }

        public UpdateQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
            UplineRoleId = user.LevelId;
            SiteDbGroupId = user.SiteDbGroupId;
        }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public string State { get; set; }

        public string Schedule { get; set; }

        public string Old_TransferOption { get; set; }

        public int UplineId { get; }

        public int SubAccId { get; }

        public byte UplineRoleId { get; set; }

        public string IP { get; }

        public string Language { get; }

        public int SiteDbGroupId { get; set; }

        public string[] DateSchedule => (Schedule ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public bool IsValid() => CustId > 0 && UplineId > 0 && (State == "off" || DateSchedule.Any());

        public string HintPath => Resources.ResourceManager.GetString("AutoTransfer_HintPath_" + UplineRoleId);
        public List<int> TransferrableDates => TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(Old_TransferOption, UplineRoleId);
    }
}