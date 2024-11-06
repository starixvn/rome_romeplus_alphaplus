using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory
{
    public class TransferToDownlineHistoryExcelQuery : TransferToDownlineHistoryBaseQuery, IRequest<ExcelFileViewModel>
    {
        public TransferToDownlineHistoryExcelQuery() : this(null) { }

        public TransferToDownlineHistoryExcelQuery(IUser viewer) : base(viewer)
        {
        }
    }
}