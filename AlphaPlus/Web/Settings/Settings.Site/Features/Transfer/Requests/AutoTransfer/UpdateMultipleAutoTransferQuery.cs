namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    using System.Collections.Generic;
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using AlphaPlus.Settings.Transfer.API.Helpers;
    using Settings.Transfer.API.Models;

    public class UpdateMultipleAutoTransferQuery
    {
        public UpdateMultipleAutoTransferQuery() : this(null) { }

        public UpdateMultipleAutoTransferQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            Language = user.Language;
        }

        public string UplineCondition { get; set; }

        public string OnIds { get; set; }

        public string OnNames { get; set; }

        public string OffIds { get; set; }

        public string OffNames { get; set; }

        public string Language { get; set; }

        public bool IsMixStatus => !string.IsNullOrWhiteSpace(OnIds) && !string.IsNullOrWhiteSpace(OffIds);
        public List<int> TransferrableDates => TransferConditionHelper.GetTransferrableDatesFromDates(UplineCondition);
        public string Old_TransferOption => TransferConditionHelper.GetTransferOption(TransferrableDates);
    }
}