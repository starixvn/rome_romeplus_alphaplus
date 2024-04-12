namespace AlphaPlus.Settings.Site.Transfer.Requests.AutoTransfer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.Transfer.API.Helpers;
    using MediatR;
    using Settings.Transfer.API.Models;
    using AlphaPlus.Settings.Transfer.API.AppResources;

    public class UpdateQuery : IRequest<Result>
    {
        public UpdateQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
            UplineRoleId = user.LevelId;
            AGIP = user.AGIP;
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

        public string AGIP { get; }

        public string[] DateSchedule => (Schedule ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        public bool IsValid() => CustId > 0 && UplineId > 0 && (State == "off" || DateSchedule.Any());

        public string HintPath => Resources.ResourceManager.GetString("AutoTransfer_HintPath_" + UplineRoleId);
        public List<int> TransferrableDates => TransferConditionHelper.GetAutoTransferScheduleFromTransferOption(Old_TransferOption, UplineRoleId);
    }
}