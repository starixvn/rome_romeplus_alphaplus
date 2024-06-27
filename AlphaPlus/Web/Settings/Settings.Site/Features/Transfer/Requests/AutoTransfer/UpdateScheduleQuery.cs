namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using AlphaPlus.Settings.Transfer.API.Helpers;
    using Settings.Transfer.API.Models;

    public class UpdateScheduleQuery
    {
        public UpdateScheduleQuery() : this(null) { }

        public UpdateScheduleQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
        }

        public int CustId { get; set; }
        public string CustName { get; set; }
        public string Schedule { get; set; }
        public string UplineCondition { get; set; }

        public int UplineId { get; }
        public int SubAccId { get; }
        public string IP { get; }
        public string Language { get; }

        public List<int> ScheduleDates => TransferConditionHelper.GetTransferrableDatesFromSchedule(Schedule);
        public List<int> TransferrableDates => (UplineCondition ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(d => int.Parse(d)).ToList();

        public string Old_TransferOption => TransferConditionHelper.GetTransferOption(TransferrableDates);

        public bool IsValid() => CustId > 0 && UplineId > 0 && ScheduleDates.Any() && TransferrableDates.Any();
    }
}