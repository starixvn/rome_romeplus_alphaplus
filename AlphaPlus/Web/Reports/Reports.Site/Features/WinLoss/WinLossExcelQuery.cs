using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
{
}