namespace AlphaPlus.Settings.Site.Transfer.Requests.TransferToDownlineHistory
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Settings.Transfer.API.Configurations;
    using AlphaPlus.Settings.Transfer.API.Models;

    public class TransferToDownlineHistoryBaseQuery : TransferBaseQuery
    {
        public TransferToDownlineHistoryBaseQuery(IUser viewer) : base(viewer)
        {
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