using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class BetCreditHistoryExcelQuery : BetCreditHistoryQuery, IRequest<ExcelFileViewModel>
{
    public BetCreditHistoryExcelQuery()
        => PageSize = int.MaxValue - 1;
}
