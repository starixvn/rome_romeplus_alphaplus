using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class MoveStatementHistoryExcelQuery : MoveStatementHistoryQuery, IRequest<ExcelFileViewModel>
{
}
