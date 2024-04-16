namespace AlphaPlus.Settings.Site.Transfer.Requests.TransferToDownlineHistory
{
    using AlphaPlus.Settings.Transfer.API.Models;
    using MediatR;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryPagingQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryPagingViewModel>
    {
        public TransferToDownlineHistoryPagingQuery() : this(null) {}
        
        public TransferToDownlineHistoryPagingQuery(IUser viewer) : base(viewer)
        {
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}