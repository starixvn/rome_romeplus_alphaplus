namespace AlphaPlus.Settings.Site.Transfer.Requests.AutoTransfer
{
    using Microsoft.AspNetCore.Mvc;
    using AlphaPlus.Settings.Transfer.API.Models.AutoTransfer;
    using MediatR;
    using Settings.Transfer.API.Models;

    public class GetTodayTransfersQuery : IRequest<AutoTransferMonitoringPageModel>
    {
        public GetTodayTransfersQuery(IUser user)
        {
            CustId = user.Id;
            Language = user.LanguageKey;
            AGIP = user.AGIP;
        }

        public int CustId { get; }

        public string Language { get; set; }

        
        public string Username { get; set; }

        public string Status { get; set; }

        public int SortedBy { get; set; }

        public int Direction { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string AGIP { get; }

        public bool IsValid() => CustId > 0;
    }
}