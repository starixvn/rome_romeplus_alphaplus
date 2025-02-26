using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferToDownlineHistory;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory
{
    public class TransferToDownlineHistoryQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryQuery() : this(null) { }

        public TransferToDownlineHistoryQuery(IUser viewer) : base(viewer)
        {
        }
    }
}