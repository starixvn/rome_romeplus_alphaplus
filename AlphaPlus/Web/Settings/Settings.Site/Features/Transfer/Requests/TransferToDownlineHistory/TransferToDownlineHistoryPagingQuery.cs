using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory
{
    public class TransferToDownlineHistoryPagingQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryPagingViewModel>
    {
        public TransferToDownlineHistoryPagingQuery() : this(null) { }

        public TransferToDownlineHistoryPagingQuery(IUser viewer) : base(viewer)
        {
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}