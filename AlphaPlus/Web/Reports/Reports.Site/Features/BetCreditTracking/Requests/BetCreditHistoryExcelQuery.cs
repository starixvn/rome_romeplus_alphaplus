using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.BetCreditTracking;

public class BetCreditHistoryExcelQuery : BetCreditHistoryQuery, IRequest<ExcelFileViewModel>
{
}
