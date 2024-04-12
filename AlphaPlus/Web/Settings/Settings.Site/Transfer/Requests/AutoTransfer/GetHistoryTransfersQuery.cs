namespace AlphaPlus.Settings.Site.Transfer.Requests.AutoTransfer
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
    using MediatR;
    using Settings.Transfer.API.Models;

    public class GetHistoryTransfersQuery : IRequest<AutoTransferMonitoringPageModel>
    {
        public GetHistoryTransfersQuery(IUser user)
        {
            CustId = user.Id;
            Language = user.LanguageKey;
            LimitedDateToShowInfo = user.LimitedDateToShowInfo;
        }

        public int CustId { get; }

        public string Language { get; set; }

        public DateTime LimitedDateToShowInfo { get; }

        
        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsValid() => CustId > 0;
    }
}