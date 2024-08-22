namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory
{
    using System;
    using AlphaPlus.Settings.Transfer.API.Configurations;
    using AlphaPlus.Settings.Transfer.API.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using AlphaPlus.Core.Models;

    public class TransferToDownlineHistoryBaseQuery : TransferBaseQuery
    {
        public TransferToDownlineHistoryBaseQuery() : this(null) {}

        public TransferToDownlineHistoryBaseQuery(IUser viewer) : base(viewer)
        {
            viewer = viewer ?? new User(UserSession.Current);
            LimitedDateToShowInfo = viewer.LimitedDateToShowInfo;
            Site = viewer.Site;
        }

        
        public string UserName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        public DateTime ToDate { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public string Site { get; }

        public override bool IsValid()
            => base.IsValid()
            && AppSettings.IsAllowedTransferToDownlineHistorySite(Site);
    }
}