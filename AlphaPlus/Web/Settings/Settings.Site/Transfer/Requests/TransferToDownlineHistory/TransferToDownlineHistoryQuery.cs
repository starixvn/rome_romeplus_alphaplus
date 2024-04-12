namespace AlphaPlus.Settings.Site.Transfer.Requests.TransferToDownlineHistory
{
    using AlphaPlus.Settings.Transfer.API.Models;
    using MediatR;
    using ViewModels.TransferToDownlineHistory;

    public class TransferToDownlineHistoryQuery : TransferToDownlineHistoryBaseQuery, IRequest<TransferToDownlineHistoryViewModel>
    {
        public TransferToDownlineHistoryQuery(IUser viewer) : base(viewer)
        {
        }
    }
}