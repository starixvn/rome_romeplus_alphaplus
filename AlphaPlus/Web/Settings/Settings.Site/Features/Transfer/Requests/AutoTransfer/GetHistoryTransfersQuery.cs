namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.AutoTransfer
{
    using System;
    using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
    using MediatR;
    using Settings.Transfer.API.Models;
    using AlphaPlus.Settings.Site.Features.Transfer.Models;
    using AlphaPlus.Core.Models;

    public class GetHistoryTransfersQuery : IRequest<AutoTransferMonitoringPageModel>
    {
        public GetHistoryTransfersQuery() : this(null) {}

        public GetHistoryTransfersQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
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