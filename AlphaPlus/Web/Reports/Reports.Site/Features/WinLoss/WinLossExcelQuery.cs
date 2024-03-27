using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using MediatR;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class WinLossExcelQuery : WinLossQuery, IRequest<ExcelFileViewModel>
{
}