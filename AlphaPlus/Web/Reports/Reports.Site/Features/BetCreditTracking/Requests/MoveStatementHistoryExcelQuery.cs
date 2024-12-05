using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class MoveStatementHistoryExcelQuery : MoveStatementHistoryQuery, IRequest<ExcelFileViewModel>
{
    public MoveStatementHistoryExcelQuery()
        => PageSize = int.MaxValue - 1;
}
