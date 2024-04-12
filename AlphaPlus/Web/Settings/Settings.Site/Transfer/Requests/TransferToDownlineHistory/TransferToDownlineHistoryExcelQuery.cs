namespace AlphaPlus.Settings.Site.Transfer.Requests.TransferToDownlineHistory
{
    using AlphaPlus.Settings.Transfer.API.Models;
    using MediatR;
    using ViewModels;

    public class TransferToDownlineHistoryExcelQuery : TransferToDownlineHistoryBaseQuery, IRequest<ExcelFileViewModel>
    {
        public TransferToDownlineHistoryExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}