namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory
{
    using AlphaPlus.Settings.Transfer.API.Models;
    using MediatR;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryQuery() : this(null) {}
        
        public TransferToDownlineHistoryQuery(IUser viewer) : base(viewer)
        {
        }
    }
}