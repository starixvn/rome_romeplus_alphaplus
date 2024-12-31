using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WhiteLabel;

public class DailyWinLossExcelQuery : DailyWinLossGetDataQuery, IRequest<ExcelFileViewModel>
{
}
