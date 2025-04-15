using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferToDownlineHistory;

public class TransferToDownlineHistoryExcelQuery : TransferToDownlineHistoryPagingQuery, IRequest<ExcelFileViewModel>
{
    public TransferToDownlineHistoryExcelQuery()
    {
        PageSize = 0;
        PageIndex = 0;
    }
}